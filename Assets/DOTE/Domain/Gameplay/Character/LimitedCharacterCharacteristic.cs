using UnityEngine;

namespace DOTE.Gameplay.Domain.Character
{
    public abstract class ALimitedCharacterCharacteristic<T>
    {
        public T MaxValue { get; private set; }
        public T CurrentValue { get; protected set; }
        public bool CanChangeValue { get; private set; }

        protected T startValue;

        public ALimitedCharacterCharacteristic(T maxValue, T currentValue)
        {
            startValue = currentValue;
            MaxValue = maxValue;
            SetCanChangeValue(true);
            SetCurrentValue(currentValue);
        }

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
                SetCurrentValueHook(value, ignoreMax);
            }
        }

        public void IncreaseCurrentValue(T value, bool ignoreMax = false)
        {
            if (CanChangeValue)
            {
                IncreaseCurrentValueHook(value, ignoreMax);
            }
        }

        public void DecreaseCurrentValue(T value, bool ignoreMax = false)
        {
            if (CanChangeValue)
            {
                DecreaseCurrentValueHook(value);
            }
        }

        public void SetCanChangeValue(bool value)
        {
            CanChangeValue = value;
        }

        public void ResetCurrentValue()
        {
            if (CanChangeValue)
            {
                CurrentValue = MaxValue;
            }
        }

        public abstract void ToStartValueIfLower();
        protected abstract void IncreaseCurrentValueHook(T value, bool ignoreMax = false);
        protected abstract void DecreaseCurrentValueHook(T value);
        protected abstract void SetCurrentValueHook(T value, bool ignoreMax = false);
    }

    public class FloatLimitedCharacterCharacteristic : ALimitedCharacterCharacteristic<float>
    {
        public FloatLimitedCharacterCharacteristic(float maxValue, float currentValue) : base(maxValue, currentValue)
        {
        }

        public FloatLimitedCharacterCharacteristic(float maxValue) : base(maxValue)
        {
        }

        public override void ToStartValueIfLower()
        {
            if (CurrentValue < startValue)
            {
                SetCurrentValue(startValue);
            }
        }
        protected override void IncreaseCurrentValueHook(float value, bool ignoreMax)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + value, 0, ignoreMax ? float.MaxValue : MaxValue);
        }
        protected override void DecreaseCurrentValueHook(float value)
        {
            CurrentValue = Mathf.Clamp(CurrentValue - value, 0, float.MaxValue);
        }

        protected override void SetCurrentValueHook(float value, bool ignoreMax)
        {
            CurrentValue = Mathf.Clamp(value, 0, ignoreMax ? float.MaxValue : MaxValue);
        }
    }

    public class IntLimitedCharacterCharacteristic : ALimitedCharacterCharacteristic<int>
    {
        public IntLimitedCharacterCharacteristic(int maxValue, int currentValue) : base(maxValue, currentValue)
        {
        }

        public IntLimitedCharacterCharacteristic(int maxValue) : base(maxValue)
        {
        }

        public override void ToStartValueIfLower()
        {
            if (CurrentValue < startValue)
            {
                SetCurrentValue(startValue);
            }
        }
        protected override void IncreaseCurrentValueHook(int value, bool ignoreMax)
        {
            int potentialValue = CurrentValue + value;
            CurrentValue = Mathf.Clamp(potentialValue, 0, ignoreMax ? int.MaxValue : MaxValue);
        }
        protected override void DecreaseCurrentValueHook(int value)
        {
            CurrentValue = Mathf.Clamp(CurrentValue - value, 0, int.MaxValue);
        }
        protected override void SetCurrentValueHook(int value, bool ignoreMax)
        {
            CurrentValue = Mathf.Clamp(value, 0, ignoreMax ? int.MaxValue : MaxValue);
        }
    }
}