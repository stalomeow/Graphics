using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VFX.Block;
using UnityEngine;
using UnityEngine.VFX;

namespace UnityEditor.VFX
{
    [VFXInfo]
    class VFXCubeTestOutput : VFXAbstractParticleOutput
    {
        public override string name { get { return "Cube test Output"; } }
        public override string codeGeneratorTemplate { get { return "VFXShaders/VFXParticleCube"; } }
        public override VFXTaskType taskType { get { return VFXTaskType.kParticleHexahedronOutput; } }

        [VFXSetting, SerializeField]
        bool useRimLight = false;

        [VFXSetting, SerializeField]
        bool useNormalMap = false;

        public override IEnumerable<VFXAttributeInfo> attributes
        {
            get
            {
                yield return new VFXAttributeInfo(VFXAttribute.Position, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Color, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Alpha, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Alive, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Front, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Side, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Up, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Angle, VFXAttributeMode.Read);
                yield return new VFXAttributeInfo(VFXAttribute.Pivot, VFXAttributeMode.Read);
                foreach (var size in VFXBlockUtility.GetReadableSizeAttributes(GetData()))
                    yield return size;
            }
        }

        protected override IEnumerable<VFXNamedExpression> CollectGPUExpressions(IEnumerable<VFXNamedExpression> slotExpressions)
        {
            foreach (var exp in base.CollectGPUExpressions(slotExpressions))
                yield return exp;

            if (useRimLight)
            {
                yield return slotExpressions.First(o => o.name == "rimColor");
                yield return slotExpressions.First(o => o.name == "rimCoef");

                if (useNormalMap)
                    yield return slotExpressions.First(o => o.name == "normalMap");
            }
        }

        protected override IEnumerable<VFXPropertyWithValue> inputProperties
        {
            get
            {
                var properties = base.inputProperties;
                if (useRimLight)
                {
                    properties = properties.Concat(PropertiesFromType("RimLightInputProperties"));
                    if (useNormalMap)
                        properties = properties.Concat(PropertiesFromType("NormalInputProperties"));
                }
                return properties;
            }
        }

        public class NormalInputProperties
        {
            public Texture2D normalMap;
        }

        public class RimLightInputProperties
        {
            public Color rimColor;
            public float rimCoef;
        }

        public override IEnumerable<string> additionalDefines
        {
            get
            {
                foreach (var d in base.additionalDefines)
                    yield return d;

                if (useRimLight)
                {
                    yield return "VFX_USE_RIM_LIGHT";
                    if (useNormalMap)
                        yield return "VFX_USE_NORMAL_MAP";
                }
            }
        }


        protected override IEnumerable<string> filteredOutSettings
        {
            get
            {
                if (!useRimLight)
                    yield return "useNormalMap";
            }
        }
    }
}
