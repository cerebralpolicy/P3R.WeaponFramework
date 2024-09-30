using System.Reflection;

namespace P3R.WeaponFramework.Enums;

public abstract class WFFlagEngine<TEnum, TValue>
    where TEnum : WFFlagEnumBase<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    protected static IEnumerable<TEnum> GetFlagEnumValues(TValue value, IEnumerable<TEnum> allEnumList)
    {
        GuardAgainstNull(value);
        GuardAgainstInvalidInputValue(value);
        GuardAgainstNegativeInputValue(value);

#pragma warning disable CS8604 // Possible null reference argument.
        var inputValueAsInt = int.Parse(value.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
        var enumFlagStateDictionary = new Dictionary<TEnum, bool>();
        var inputEnumList = allEnumList.ToList();

        ApplyUnsafeFlagEnumAttributeSettings(inputEnumList);

        var maximumAllowedValue = CalculateHighestAllowedFlagValue(inputEnumList);

        var typeMaxValue = GetMaxValue();

        foreach (var enumValue in inputEnumList)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var currentEnumValueAsInt = int.Parse(enumValue.Value.ToString());
#pragma warning restore CS8604 // Possible null reference argument.

            CheckEnumForNegativeValues(currentEnumValueAsInt);

            if (currentEnumValueAsInt == inputValueAsInt)
                return new List<TEnum> { enumValue };

            if (inputValueAsInt == -1 || value.Equals(typeMaxValue))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                return inputEnumList.Where(x => long.Parse(x.Value.ToString()) > 0);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            AssignFlagStateValuesToDictionary(inputValueAsInt, currentEnumValueAsInt, enumValue, enumFlagStateDictionary);
        }

#pragma warning disable CS8603 // Possible null reference return.
        return inputValueAsInt > maximumAllowedValue ? default : CreateSmartEnumReturnList(enumFlagStateDictionary);
#pragma warning restore CS8603 // Possible null reference return.

    }

    private static void GuardAgainstNull(TValue value)
    {
        if (value == null)
            ThrowHelper.ThrowArgumentNullException(nameof(value));
    }
    private static void GuardAgainstInvalidInputValue(TValue value)
    {
        if (!int.TryParse(value.ToString(), out _))
            ThrowHelper.ThrowInvalidValueCastException<TEnum, TValue>(value);
    }

    private static void GuardAgainstNegativeInputValue(TValue value)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        if (int.Parse(value.ToString()) < -1)
            ThrowHelper.ThrowNegativeValueArgumentException<TEnum, TValue>(value);
#pragma warning restore CS8604 // Possible null reference argument.
    }
    private static void CheckEnumForNegativeValues(int value)
    {
        if (value < -1)
            ThrowHelper.ThrowContainsNegativeValueException<TEnum, TValue>();
    }

    private static int CalculateHighestAllowedFlagValue(List<TEnum> inputEnumList)
    {
        return (HighestFlagValue(inputEnumList) * 2) - 1;
    }

    private static void AssignFlagStateValuesToDictionary(int inputValueAsInt, int currentEnumValue, TEnum enumValue, IDictionary<TEnum, bool> enumFlagStateDictionary)
    {
        if (!enumFlagStateDictionary.ContainsKey(enumValue) && currentEnumValue != 0)
        {
            bool flagState = (inputValueAsInt & currentEnumValue) == currentEnumValue;
            enumFlagStateDictionary.Add(enumValue, flagState);
        }
    }

    private static IEnumerable<TEnum> CreateSmartEnumReturnList(Dictionary<TEnum, bool> enumFlagStateDictionary)
    {
        var outputList = new List<TEnum>();

        foreach (var entry in enumFlagStateDictionary)
        {
            if (entry.Value)
            {
                outputList.Add(entry.Key);
            }
        }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return outputList.DefaultIfEmpty();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }
    private static bool IsPowerOfTwo(int input)
    {
        if (input != 0 && ((input & (input - 1)) == 0))
        {
            return true;
        }

        return false;
    }

    private static void ApplyUnsafeFlagEnumAttributeSettings(IEnumerable<TEnum> list)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        AllowUnsafeFlagEnumValuesAttribute attribute = (AllowUnsafeFlagEnumValuesAttribute)
            Attribute.GetCustomAttribute(typeof(TEnum), typeof(AllowUnsafeFlagEnumValuesAttribute));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (attribute == null)
        {
            CheckEnumListForPowersOfTwo(list);
        }
    }

    private static void CheckEnumListForPowersOfTwo(IEnumerable<TEnum> enumEnumerable)
    {
        var enumList = enumEnumerable.ToList();
        var enumValueList = new List<int>();
        foreach (var smartFlagEnum in enumList)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            enumValueList.Add(int.Parse(smartFlagEnum.Value.ToString()));
#pragma warning restore CS8604 // Possible null reference argument.
        }
        var firstPowerOfTwoValue = 0;
#pragma warning disable CS8604 // Possible null reference argument.
        if (int.Parse(enumList[0].Value.ToString()) == 0)
        {
            enumList.RemoveAt(0);
        }
#pragma warning restore CS8604 // Possible null reference argument.

        foreach (var flagEnum in enumList)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var x = int.Parse(flagEnum.Value.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
            if (IsPowerOfTwo(x))
            {
                firstPowerOfTwoValue = x;
                break;
            }
        }

        var highestValue = HighestFlagValue(enumList);
        var currentValue = firstPowerOfTwoValue;

        while (currentValue != highestValue)
        {
            var nextPowerOfTwoValue = currentValue * 2;
            var result = enumValueList.BinarySearch(nextPowerOfTwoValue);
            if (result < 0)
            {
                ThrowHelper.ThrowDoesNotContainPowerOfTwoValuesException<TEnum, TValue>();
            }

            currentValue = nextPowerOfTwoValue;
        }
    }
    private static int HighestFlagValue(IReadOnlyList<TEnum> enumList)
    {
        var highestIndex = enumList.Count - 1;
#pragma warning disable CS8604 // Possible null reference argument.
        var highestValue = int.Parse(enumList.Last().Value.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
        if (!IsPowerOfTwo(highestValue))
        {
            for (var i = highestIndex; i >= 0; i--)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                var currentValue = int.Parse(enumList[i].Value.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                if (IsPowerOfTwo(currentValue))
                {
                    highestValue = currentValue;
                    break;
                }
            }
        }

        return highestValue;
    }
    private static TValue GetMaxValue()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        FieldInfo maxValueField = typeof(TValue).GetField("MaxValue", BindingFlags.Public
            | BindingFlags.Static);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        if (maxValueField == null)
            throw new NotSupportedException(typeof(TValue).Name);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        TValue maxValue = (TValue)maxValueField.GetValue(null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8603 // Possible null reference return.
        return maxValue;
#pragma warning restore CS8603 // Possible null reference return.
    }
#pragma warning restore CS8604 // Possible null reference argument.
}
[AttributeUsage(AttributeTargets.Class)]
public class AllowUnsafeFlagEnumValuesAttribute : Attribute
{
}