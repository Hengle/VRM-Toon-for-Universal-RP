﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph.Drawing;
using UnityEditor.ShaderGraph.Drawing.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.ShaderGraph
{
    [Serializable]
    [Title("Master", "Toon")]
    class ToonMasterNode : MasterNode<IToonSubShader>, IMayRequirePosition, IMayRequireNormal
    {
        public const string AlbedoSlotName = "Albedo";
        public const string NormalSlotName = "Normal";
        public const string EmissionSlotName = "Emission";
        public const string SpecularSlotName = "Specular";
        public const string SmoothnessSlotName = "Smoothness";
        public const string OcclusionSlotName = "Occlusion";
        public const string AlphaSlotName = "Alpha";
        public const string AlphaClipThresholdSlotName = "AlphaClipThreshold";
        public const string PositionName = "Position";

        public const string ShadeSlotName = "Shade";
        public const string ShadeShiftSlotName = "ShadeShift";
        public const string ShadeToonySlotName = "ShadeToony";
        public const string OutlineWidthSlotName = "OutlineWidth";
        public const string ToonyLightingSlotName = "ToonyLighting";
        public const string SphereAddSlotName = "SphereAdd";
        public const string OutlineColorSlotName = "OutlineColor";

        public const int AlbedoSlotId = 0;
        public const int NormalSlotId = 1;
        public const int SpecularSlotId = 2;
        public const int EmissionSlotId = 3;
        public const int SmoothnessSlotId = 4;
        public const int OcclusionSlotId = 5;
        public const int AlphaSlotId = 6;
        public const int AlphaThresholdSlotId = 7;
        public const int PositionSlotId = 8;

        public const int ShadeSlotId = 9;
        public const int ShadeShiftSlotId = 10;
        public const int ShadeToonySlotId = 11;
        public const int OutlineWidthSlotId = 12;
        public const int ToonyLightingSlotId = 13;
        public const int SphereAddSlotId = 14;

        public const int OutlineColorSlotId = 15;

        public enum Model
        {
            Specular
        }

        [SerializeField]
        Model m_Model = Model.Specular;

        public Model model
        {
            get { return m_Model; }
            set
            {
                if (m_Model == value)
                    return;

                m_Model = value;
                UpdateNodeAfterDeserialization();
                Dirty(ModificationScope.Topological);
            }
        }

        [SerializeField]
        SurfaceType m_SurfaceType;

        public SurfaceType surfaceType
        {
            get { return m_SurfaceType; }
            set
            {
                if (m_SurfaceType == value)
                    return;

                m_SurfaceType = value;
                Dirty(ModificationScope.Graph);
            }
        }

        [SerializeField]
        AlphaMode m_AlphaMode;

        public AlphaMode alphaMode
        {
            get { return m_AlphaMode; }
            set
            {
                if (m_AlphaMode == value)
                    return;

                m_AlphaMode = value;
                Dirty(ModificationScope.Graph);
            }
        }

        [SerializeField]
        bool m_TwoSided;

        public ToggleData twoSided
        {
            get { return new ToggleData(m_TwoSided); }
            set
            {
                if (m_TwoSided == value.isOn)
                    return;
                m_TwoSided = value.isOn;
                Dirty(ModificationScope.Graph);
            }
        }

        public ToonMasterNode()
        {
            UpdateNodeAfterDeserialization();
        }

        public override string documentationURL
        {
            get { return "https://simplestar-tech.hatenablog.com/entry/2019/07/15/110051"; }
        }

        public sealed override void UpdateNodeAfterDeserialization()
        {
            base.UpdateNodeAfterDeserialization();
            name = "Toon Master";
            AddSlot(new PositionMaterialSlot(PositionSlotId, PositionName, PositionName, CoordinateSpace.Object, ShaderStageCapability.Vertex));
            AddSlot(new ColorRGBMaterialSlot(AlbedoSlotId, AlbedoSlotName, AlbedoSlotName, SlotType.Input, Color.grey.gamma, ColorMode.Default, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(AlphaSlotId, AlphaSlotName, AlphaSlotName, SlotType.Input, 1f, ShaderStageCapability.Fragment));
            AddSlot(new ColorRGBMaterialSlot(ShadeSlotId, ShadeSlotName, ShadeSlotName, SlotType.Input, Color.gray, ColorMode.Default, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(ShadeShiftSlotId, ShadeShiftSlotName, ShadeShiftSlotName, SlotType.Input, 0.5f, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(ShadeToonySlotId, ShadeToonySlotName, ShadeToonySlotName, SlotType.Input, 0.8f, ShaderStageCapability.Fragment));
            AddSlot(new NormalMaterialSlot(NormalSlotId, NormalSlotName, NormalSlotName, CoordinateSpace.Tangent, ShaderStageCapability.Fragment));
            AddSlot(new ColorRGBMaterialSlot(SphereAddSlotId, SphereAddSlotName, SphereAddSlotName, SlotType.Input, Color.black, ColorMode.Default, ShaderStageCapability.Fragment));
            AddSlot(new ColorRGBMaterialSlot(EmissionSlotId, EmissionSlotName, EmissionSlotName, SlotType.Input, Color.black, ColorMode.Default, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(AlphaThresholdSlotId, AlphaClipThresholdSlotName, AlphaClipThresholdSlotName, SlotType.Input, 0.5f, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(OutlineWidthSlotId, OutlineWidthSlotName, OutlineWidthSlotName, SlotType.Input, 1f, ShaderStageCapability.Vertex));
            AddSlot(new Vector1MaterialSlot(ToonyLightingSlotId, ToonyLightingSlotName, ToonyLightingSlotName, SlotType.Input, 1f, ShaderStageCapability.Fragment));
            AddSlot(new ColorRGBMaterialSlot(SpecularSlotId, SpecularSlotName, SpecularSlotName, SlotType.Input, Color.grey, ColorMode.Default, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(SmoothnessSlotId, SmoothnessSlotName, SmoothnessSlotName, SlotType.Input, 0.5f, ShaderStageCapability.Fragment));
            AddSlot(new Vector1MaterialSlot(OcclusionSlotId, OcclusionSlotName, OcclusionSlotName, SlotType.Input, 1f, ShaderStageCapability.Fragment));
            AddSlot(new ColorRGBMaterialSlot(OutlineColorSlotId, OutlineColorSlotName, OutlineColorSlotName, SlotType.Input, Color.black, ColorMode.Default, ShaderStageCapability.Fragment));

            // clear out slot names that do not match the slots
            // we support
            RemoveSlotsNameNotMatching(
                new[]
            {
                PositionSlotId,
                AlbedoSlotId,
                ShadeSlotId,
                ShadeShiftSlotId,
                ShadeToonySlotId,
                NormalSlotId,
                EmissionSlotId,
                AlphaSlotId,
                AlphaThresholdSlotId,
                OutlineWidthSlotId,
                ToonyLightingSlotId,
                SphereAddSlotId,
                SpecularSlotId,
                SmoothnessSlotId,
                OcclusionSlotId,
                OutlineColorSlotId,
            }, true);
        }

        protected override VisualElement CreateCommonSettingsElement()
        {
            return new ToonSettingsView(this);
        }

        public NeededCoordinateSpace RequiresNormal(ShaderStageCapability stageCapability)
        {
            List<MaterialSlot> slots = new List<MaterialSlot>();
            GetSlots(slots);

            List<MaterialSlot> validSlots = new List<MaterialSlot>();
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].stageCapability != ShaderStageCapability.All && slots[i].stageCapability != stageCapability)
                    continue;

                validSlots.Add(slots[i]);
            }
            return validSlots.OfType<IMayRequireNormal>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresNormal(stageCapability));
        }

        public NeededCoordinateSpace RequiresPosition(ShaderStageCapability stageCapability)
        {
            List<MaterialSlot> slots = new List<MaterialSlot>();
            GetSlots(slots);

            List<MaterialSlot> validSlots = new List<MaterialSlot>();
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].stageCapability != ShaderStageCapability.All && slots[i].stageCapability != stageCapability)
                    continue;

                validSlots.Add(slots[i]);
            }
            return validSlots.OfType<IMayRequirePosition>().Aggregate(NeededCoordinateSpace.None, (mask, node) => mask | node.RequiresPosition(stageCapability));
        }
    }
}
