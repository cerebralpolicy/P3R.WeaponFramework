﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Interfaces.Types;
public class ArmatureTargets : Collection<EArmature>
{
    public ArmatureTargets()
    {
    }

    public ArmatureTargets(IList<EArmature> list) : base(list)
    {
    }
}
public class ArmatureTuple : Tuple<ShellType, ArmatureTargets>
{
    public ArmatureTuple(ShellType item1, ArmatureTargets item2) : base(item1, item2)
    {
    }
}

public class ArmatureTuples : Collection<ArmatureTuple>
{
    public ArmatureTuples()
    {
    }

    public ArmatureTuples(IList<ArmatureTuple> list) : base(list)
    {
    }
}

public class ArmatureSets : Dictionary<ECharacter,ArmatureTuples>;
public class WeaponIdTable<T> : Dictionary<int,T> where T : IWeapon;

public class CategorizedWeaponTable<T> : Dictionary<ECharacter, WeaponIdTable<T>> where T : IWeapon;
