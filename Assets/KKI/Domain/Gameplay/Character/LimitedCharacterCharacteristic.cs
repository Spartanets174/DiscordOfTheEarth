using UnityEngine;

namespace DOTE.Domain.Gameplay.Character
{
    public abstract class ALimitedCharacterCharacteristic<T>
    {
        public T MaxValue { get; private set; }
        public T CurrentValue { get; protected set; }
        public bool CanChangeValue { get; private set; }

        public ALimitedCharacterCharacteristic(T maxValue)
        {
            MaxValue = maxValue;
            SetCanChangeValue(true);
            SetCurrentValue(MaxValue);
        }

        public void SetCurrentValue(T value, bool ignoreMax = false)
        {
            if (CanChangeValue)
            {
                SetCurrentValueHook(value);
            }
        }

        public void IncreaseCurrentValue(T value, bool ignoreMax = false)
        {
            if (CanChangeValue)
            {
                IncreaseCurrentValueHook(value);
            }
        }

        public void SetCanChangeValue(bool value)
        {
            CanChangeValue = value;
        }

        public void ResetCurrentValue()
        {
            CurrentValue = MaxValue;
        }

        protected abstract void IncreaseCurrentValueHook(T value, bool ignoreMax = false);
        protected abstract void SetCurrentValueHook(T value, bool ignoreMax = false);
    }

    public class FloatLimitedCharacterCharacteristic : ALimitedCharacterCharacteristic<float>
    {
        public FloatLimitedCharacterCharacteristic(float maxValue) : base(maxValue)
        {
        }

        protected override void IncreaseCurrentValueHook(float value, bool ignoreMax)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + value, 0, ignoreMax ? float.MaxValue : MaxValue);
        }

        protected override void SetCurrentValueHook(float value, bool ignoreMax)
        {
            CurrentValue = Mathf.Clamp(value, 0, ignoreMax ? float.MaxValue : MaxValue);
        }
    }

    public class IntLimitedCharacterCharacteristic : ALimitedCharacterCharacteristic<int>
    {
        public IntLimitedCharacterCharacteristic(int maxValue) : base(maxValue)
        {
        }
        protected override void IncreaseCurrentValueHook(int value, bool ignoreMax)
        {
            int potentialValue = CurrentValue + value;
            CurrentValue = Mathf.Clamp(potentialValue, 0, ignoreMax ? int.MaxValue : MaxValue);
        }

        protected override void SetCurrentValueHook(int value, bool ignoreMax)
        {
            CurrentValue = Mathf.Clamp(value, 0, ignoreMax ? int.MaxValue : MaxValue);
        }
    }
}