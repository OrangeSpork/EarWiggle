using ExtensibleSaveFormat;
using KKAPI;
using KKAPI.Chara;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EarWiggle
{
    public class EarWiggleCharaController : CharaCustomFunctionController
    {
        public bool EarWiggleEnabled { get; set; } = false;
        public float Damping { get; set; } = 0.2f;
        public float Elasticity { get; set; } = 0.1f;
        public float Stiffness { get; set; } = 0.2f;
        public float Inert { get; set; } = 0.03f;
        public float GravityX { get; set; } = 0f;
        public float GravityY { get; set; } = 0f;
        public float GravityZ { get; set; } = 0f;
        public float CollisionRadius { get; set; } = 0.5f;
        public float EndOffsetX { get; set; } = .1f;
        public float EndOffsetY { get; set; } = .5f;
        public float EndOffsetZ { get; set; } = -.1f;

        private DynamicBone_Ver02 LeftEarDBone { get; set; }
        private DynamicBone_Ver02 RightEarDBone { get; set; }

        private bool Initializing = false;

        protected override void OnEnable()
        {
            if (LeftEarDBone == null || RightEarDBone == null)
            {
                if (!Initializing)
                {
                    Initializing = true;
                    StartCoroutine(BuildEars());
                }
            }
            base.OnEnable();
        }

        private IEnumerator BuildEars()
        {
            yield return new WaitUntil(() => { return ChaControl.objHeadBone != null; });
#if DEBUG
            EarWigglePlugin.Instance.Log.LogInfo($"Building Ears for: {ChaControl?.fileParam.fullname} {ChaControl?.objHeadBone}");
#endif

            Transform leftEar = FindDescendant(ChaControl.objHeadBone.transform, "cf_J_EarBase_s_L");
            Transform leftEarUpper = FindDescendant(leftEar, "cf_J_EarUp_L");
            LeftEarDBone = BuildEar(leftEar, leftEarUpper, true);
#if DEBUG
            EarWigglePlugin.Instance.Log.LogInfo("Built Left Ear");
#endif
            Transform rightEar = FindDescendant(ChaControl.objHeadBone.transform, "cf_J_EarBase_s_R");
            Transform rightEarUpper = FindDescendant(rightEar, "cf_J_EarUp_R");
            RightEarDBone = BuildEar(rightEar, rightEarUpper, false);
#if DEBUG
            EarWigglePlugin.Instance.Log.LogInfo("Built Right Ear");
#endif

            if (EarWiggleEnabled)
                EnableEarDBones();
            else
                DisableEarDBones();

            Initializing = false;
        }

        private DynamicBone_Ver02 BuildEar(Transform earBase, Transform earUpper, bool left)
        {
            earBase.gameObject.SetActive(false);

            DynamicBone_Ver02 earDBone = earBase.gameObject.AddComponent<DynamicBone_Ver02>();

            earDBone.Root = earBase;

            earDBone.Bones = new List<Transform>();
            earDBone.Bones.Add(earBase);
            earDBone.Bones.Add(earUpper);

            earDBone.Colliders = new List<DynamicBoneCollider>();
            earDBone.Patterns = new List<DynamicBone_Ver02.BonePtn>();

            DynamicBone_Ver02.BonePtn ptn = new DynamicBone_Ver02.BonePtn();

            ptn.Name = "Default";

            ptn.Params = new List<DynamicBone_Ver02.BoneParameter>();

            ptn.Gravity = new Vector3(left ? GravityX * -1 : GravityX, GravityY, GravityZ);
            ptn.EndOffset = new Vector3(left ? EndOffsetX * -1 : EndOffsetX, EndOffsetY, EndOffsetZ);
            ptn.EndOffsetDamping = Damping;
            ptn.EndOffsetElasticity = Elasticity;
            ptn.EndOffsetStiffness = Stiffness;
            ptn.EndOffsetInert = Inert;

            DynamicBone_Ver02.BoneParameter earBaseParam = new DynamicBone_Ver02.BoneParameter();

            earBaseParam.Name = earBase.name;
            earBaseParam.RefTransform = earBase;
            earBaseParam.IsRotationCalc = false;
            earBaseParam.Damping = Damping;
            earBaseParam.Elasticity = Elasticity;
            earBaseParam.Stiffness = Stiffness;
            earBaseParam.Inert = Inert;
            earBaseParam.NextBoneLength = 3f;
            earBaseParam.CollisionRadius = 0.0f;
            earBaseParam.IsMoveLimit = false;

            ptn.Params.Add(earBaseParam);

            DynamicBone_Ver02.BoneParameter earUpperParam = new DynamicBone_Ver02.BoneParameter();

            earUpperParam.Name = earUpper.name;
            earUpperParam.RefTransform = earUpper;
            earUpperParam.IsRotationCalc = true;
            earUpperParam.Damping = Damping;
            earUpperParam.Elasticity = Elasticity;
            earUpperParam.Stiffness = Stiffness;
            earUpperParam.Inert = Inert;
            earUpperParam.NextBoneLength = 10f;
            earUpperParam.CollisionRadius = CollisionRadius;
            earUpperParam.IsMoveLimit = false;

            ptn.Params.Add(earUpperParam);

            earDBone.Patterns.Add(ptn);
            earBase.gameObject.SetActive(true);                

            return earDBone;
        }

        public void UpdateEars()
        {
            if (EarWiggleMakerGUI.UpdatingGUI)
                return;

#if DEBUG
            EarWigglePlugin.Instance.Log.LogInfo($"Updating Ears {EarWiggleEnabled}");
#endif

            if (!Initializing)
            {
                UpdateEar(LeftEarDBone, true);
                UpdateEar(RightEarDBone, false);

                if (EarWiggleEnabled)
                    EnableEarDBones();
                else
                    DisableEarDBones();
            }
        }

        private void UpdateEar(DynamicBone_Ver02 ear, bool left)
        {
            foreach (DynamicBone_Ver02.BonePtn ptn in ear.Patterns)
            {
                ptn.Gravity = new Vector3(left ? GravityX * -1 : GravityX, GravityY, GravityZ);
            //    ptn.EndOffset = new Vector3(EndOffsetX, EndOffsetY, EndOffsetZ);
                ptn.EndOffsetDamping = Damping;
                ptn.EndOffsetElasticity = Elasticity;
                ptn.EndOffsetStiffness = Stiffness;
                ptn.EndOffsetInert = Inert;

                DynamicBone_Ver02.BoneParameter earBaseParam = ptn.Params[0];
                earBaseParam.Damping = Damping;
                earBaseParam.Elasticity = Elasticity;
                earBaseParam.Stiffness = Stiffness;
                earBaseParam.Inert = Inert;

                DynamicBone_Ver02.BoneParameter earUpperParam = ptn.Params[1];
                earUpperParam.Damping = Damping;
                earUpperParam.Elasticity = Elasticity;
                earUpperParam.Stiffness = Stiffness;
                earUpperParam.Inert = Inert;
                earUpperParam.CollisionRadius = CollisionRadius;

                foreach (DynamicBone_Ver02.ParticlePtn particlePtn in ptn.ParticlePtns)
                {
                    particlePtn.Damping = Mathf.Clamp01(Damping);
                    particlePtn.Elasticity = Mathf.Clamp01(Elasticity);
                    particlePtn.Inert = Mathf.Clamp01(Inert);
                    particlePtn.Radius = Mathf.Max(CollisionRadius, 0f);
                    particlePtn.Stiffness = Mathf.Clamp01(Stiffness);
                }
            }
            ear.setPtn(0, true);
        }

        private bool DoDisable = false;
        private bool DoEnable = false;
        public void DisableEarDBones()
        {
            DoDisable = true;
        }

        public void EnableEarDBones()
        {
            DoEnable = true;            
        }

        protected void LateUpdate()
        {
            if (DoEnable)
            {
                LeftEarDBone.enabled = true;
                RightEarDBone.enabled = true;
                DoEnable = false;
            }
            else if (DoDisable)
            {
                LeftEarDBone.enabled = false;
                RightEarDBone.enabled = false;
                DoDisable = false;
            }
        }

        protected override void OnCardBeingSaved(GameMode currentGameMode)
        {
            var data = new PluginData();

            data.data["EarWiggleEnabled"] = EarWiggleEnabled;
            data.data["Damping"] = Damping;
            data.data["Elasticity"] = Elasticity;
            data.data["Stiffness"] = Stiffness;
            data.data["Inert"] = Inert;
            data.data["GravityX"] = GravityX;
            data.data["GravityY"] = GravityY;
            data.data["GravityZ"] = GravityZ;
            data.data["CollisionRadius"] = CollisionRadius;

            SetExtendedData(data);
        }

        protected override void OnReload(GameMode currentGameMode, bool maintainState)
        {
            if (maintainState)
                return;

            var data = GetExtendedData();
            if (data != null)
            {
                if (data.data.TryGetValue("EarWiggleEnabled", out var val1)) EarWiggleEnabled = (bool)val1;
                if (data.data.TryGetValue("Damping", out var val2)) Damping = (float)val2;
                if (data.data.TryGetValue("Elasticity", out var val3)) Elasticity = (float)val3;
                if (data.data.TryGetValue("Stiffness", out var val4)) Stiffness = (float)val4;
                if (data.data.TryGetValue("Inert", out var val5)) Inert = (float)val5;
                if (data.data.TryGetValue("GravityX", out var val6)) GravityX = (float)val6;
                if (data.data.TryGetValue("GravityY", out var val7)) GravityY = (float)val7;
                if (data.data.TryGetValue("GravityZ", out var val8)) GravityZ = (float)val8;
                if (data.data.TryGetValue("CollisionRadius", out var val9)) CollisionRadius = (float)val9;
            }
            else
            {
                EarWiggleEnabled = false;
                Damping = 0.2f;
                Elasticity = 0.1f;
                Stiffness = 0.2f;
                Inert = 0.03f;
                CollisionRadius = 0.5f;
                GravityX = 0f;
                GravityY = 0f;
                GravityZ = 0f;
            }

            UpdateEars();
        }


        private Transform FindDescendant(Transform start, string name)
        {
            if (start == null)
            {
                return null;
            }

            if (start.name.Equals(name))
            {
                return start;
            }
            foreach (Transform t in start)
            {
                Transform res = FindDescendant(t, name);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }
    }
}
