﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Enums;

public abstract class WFEnumCollection<TEnum, TValue, EEnum> : KeyedCollection<EEnum, TEnum>
    where TEnum : WFEnumWrapper<TEnum, TValue, EEnum>
    where TValue : IEquatable<TValue>, IComparable<TValue>
    where EEnum : struct, Enum
{


    protected WFEnumCollection()
    {
    }

    protected WFEnumCollection(IEqualityComparer<EEnum>? comparer) : base(comparer)
    {
    }

    protected WFEnumCollection(IEqualityComparer<EEnum>? comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold)
    {
    }
}
