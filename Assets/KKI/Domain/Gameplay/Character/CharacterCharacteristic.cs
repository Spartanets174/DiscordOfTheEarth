using UnityEngine;

namespace DOTE.Domain.Gameplay.Character
{
    public abstract class ACharacterCharacteristic<T>
    {
        public T StartValue { get; private set; }
        public T CurrentValue { get; protected set; }
        public bool CanChangeValue { get; private set; }

        public ACharacterCharacteristic(T startValue)
        {
            StartValue = startValue;
            SetCanChangeValue(true);
            SetCurrentValue(StartValue);
        }

        public void SetCurrentValue(T value)
        {
            if (CanChangeValue)
            {
                SetCurrentValueHook(value);
            }
        }
        public void IncreaseCurrentValue(T value)
        {
            if (CanChangeValue)
            {
                IncreaseCurrentValueHook(value);
            }
        }

        public void DecreaseCurrentValue(T value)
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
                CurrentValue = StartValue;
            }
        }

        public abstract void ToStartValueIfLower();
        public abstract void ToStartValueIfHigher();
        protected abstract void DecreaseCurrentValueHook(T value);
        protected abstract void IncreaseCurrentValueHook(T value);
        protected abstract void SetCurrentValueHook(T value);
    }

    public class FloatCharacterCharacteristic : ACharacterCharacteristic<float>
    {
        public FloatCharacterCharacteristic(float startValue) : base(startValue)
        {
        }

        public override void ToStartValueIfLower()
        {
            if (CurrentValue < StartValue)
            {
                ResetCurrentValue();
            }
        }

        public override void ToStartValueIfHigher()
        {
            if (CurrentValue > StartValue)
            {
                ResetCurrentValue();
            }
        }
        protected override void IncreaseCurrentValueHook(float value)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + value, 0, float.MaxValue);
        }
        protected override void DecreaseCurrentValueHook(float value)
        {
            CurrentValue = Mathf.Clamp(CurrentValue - value, 0, float.MaxValue);
        }
        protected override void SetCurrentValueHook(float value)
        {
            CurrentValue = Mathf.Clamp(value, 0, float.MaxValue);
        }

    }

    public class IntCharacterCharacteristic : ACharacterCharacteristic<int>
    {
        public IntCharacterCharacteristic(int startValue) : base(startValue)
        {
        }
        public override void ToStartValueIfLower()
        {
            if (CurrentValue < StartValue)
            {
                ResetCurrentValue();
            }
        }
        public override void ToStartValueIfHigher()
        {
            if (CurrentValue > StartValue)
            {
                ResetCurrentValue();
            }
        }
        protected override void IncreaseCurrentValueHook(int value)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + value, 0, int.MaxValue);
        }

        protected override void DecreaseCurrentValueHook(int value)
        {
            CurrentValue = Mathf.Clamp(CurrentValue - value, 0, int.MaxValue);
        }

        protected override void SetCurrentValueHook(int value)
        {
            CurrentValue = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }
}