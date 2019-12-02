using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IMoveStrategy
{
    IEnumerable Move();

    bool IsCompleted { get; }

    YieldInstruction Delay();
}
