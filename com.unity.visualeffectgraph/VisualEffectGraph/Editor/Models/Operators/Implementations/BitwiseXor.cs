using System;
using UnityEngine;

namespace UnityEditor.VFX.Operator
{
    [VFXInfo(category = "Bitwise")]
    class BitwiseXor : VFXOperator
    {
        override public string name { get { return "Xor"; } }

        public class InputProperties
        {
            static public uint FallbackValue = 0;
            [Tooltip("The first operand.")]
            public uint a = FallbackValue;
            [Tooltip("The second operand.")]
            public uint b = FallbackValue;
        }

        public class OutputProperties
        {
            public uint o;
        }

        protected override sealed VFXExpression[] BuildExpression(VFXExpression[] inputExpression)
        {
            return new[] { new VFXExpressionBitwiseXor(inputExpression[0], inputExpression[1]) };
        }
    }
}
