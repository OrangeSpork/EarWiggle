using AIChara;
using KKAPI.Maker;
using KKAPI.Maker.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UniRx;

namespace EarWiggle
{
    public class EarWiggleMakerGUI
    {
        private static EarWiggleCharaController controller;

        public void RegisterMakerAPIControls()
        {
            MakerAPI.RegisterCustomSubCategories += RegisterCustomControls;
            MakerAPI.ReloadCustomInterface += ReloadCustomInterface;
        }

        private void ReloadCustomInterface(object sender, EventArgs e)
        {
            ChaControl character = MakerAPI.GetCharacterControl();
            controller = character?.gameObject.GetComponent<EarWiggleCharaController>();
            if (controller != null)
                UpdateEarWiggleGUI();
        }

        private static MakerCategory makerCategory;
        private void RegisterCustomControls(object sender, RegisterSubCategoriesEvent e)
        {
            makerCategory = new MakerCategory(MakerConstants.Face.Ear.CategoryName, "Ear Wiggling");
            e.AddSubCategory(makerCategory);

            ChaControl character = MakerAPI.GetCharacterControl();
            controller = character?.gameObject.GetComponent<EarWiggleCharaController>();
            InitializeEarWiggleMakerGUI();
        }

        public static bool UpdatingGUI { get; set; }
        private static MakerToggle EnableWiggleToggle;
        private static MakerSlider DampingSlider;
        private static MakerSlider ElasticitySlider;
        private static MakerSlider StiffnessSlider;
        private static MakerSlider InertiaSlider;
        private static MakerSlider GravityXSlider;
        private static MakerSlider GravityYSlider;
        private static MakerSlider GravityZSlider;
        private static MakerSlider CollisionRadiusSlider;

        public static void UpdateEarWiggleGUI()
        {
#if DEBUG
            EarWigglePlugin.Instance.Log.LogInfo($"Updating Ear Wiggle GUI for {controller} {controller?.ChaControl}");
#endif
            if (!controller)
                return;

            UpdatingGUI = true;


            EnableWiggleToggle.Value = controller.EarWiggleEnabled;
            DampingSlider.Value = controller.Damping;
            ElasticitySlider.Value = controller.Elasticity;
            StiffnessSlider.Value = controller.Stiffness;
            InertiaSlider.Value = controller.Inert;
            GravityXSlider.Value = controller.GravityX;
            GravityYSlider.Value = controller.GravityY;
            GravityZSlider.Value = controller.GravityZ;
            CollisionRadiusSlider.Value = controller.CollisionRadius;

            UpdatingGUI = false;
            controller.UpdateEars();
        }

        public static void InitializeEarWiggleMakerGUI()
        {
#if DEBUG
            EarWigglePlugin.Instance.Log.LogInfo($"Building Ear Wiggle GUI");
#endif
            UpdatingGUI = true;

            EnableWiggleToggle = new MakerToggle(makerCategory, "Enable", false, EarWigglePlugin.Instance);
            EnableWiggleToggle.ValueChanged.Subscribe(Observer.Create<bool>((b) =>
            {
                if (controller != null)
                {
                    controller.EarWiggleEnabled = b;
                    controller.UpdateEars();
                }
            }));

            DampingSlider = new MakerSlider(makerCategory, "Damping: ", 0.0f, 1.0f, 0.2f, EarWigglePlugin.Instance);
            DampingSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.Damping = f;
                    controller.UpdateEars();
                }
            }));

            ElasticitySlider = new MakerSlider(makerCategory, "Elasticity: ", 0.0f, 1.0f, 0.1f, EarWigglePlugin.Instance);
            ElasticitySlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.Elasticity = f;
                    controller.UpdateEars();
                }
            }));

            StiffnessSlider = new MakerSlider(makerCategory, "Stiffness: ", 0.0f, 1.0f, 0.2f, EarWigglePlugin.Instance);
            StiffnessSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.Stiffness = f;
                    controller.UpdateEars();
                }
            }));

            InertiaSlider = new MakerSlider(makerCategory, "Inertia: ", 0.0f, 1.0f, 0.03f, EarWigglePlugin.Instance);
            InertiaSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.Inert = f;
                    controller.UpdateEars();
                }
            }));

            GravityXSlider = new MakerSlider(makerCategory, "Gravity X: ", -10f, 10f, 0f, EarWigglePlugin.Instance);
            GravityXSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.GravityX = f;
                    controller.UpdateEars();
                }
            }));

            GravityYSlider = new MakerSlider(makerCategory, "Gravity Y: ", -10f, 10f, 0f, EarWigglePlugin.Instance);
            GravityYSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.GravityY = f;
                    controller.UpdateEars();
                }
            }));

            GravityZSlider = new MakerSlider(makerCategory, "Gravity Z: ", -10f, 10f, 0f, EarWigglePlugin.Instance);
            GravityZSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.GravityZ = f;
                    controller.UpdateEars();
                }
            }));

            CollisionRadiusSlider = new MakerSlider(makerCategory, "Col. Radius: ", 0f, 3f, 0.5f, EarWigglePlugin.Instance);
            CollisionRadiusSlider.ValueChanged.Subscribe(Observer.Create<float>((f) =>
            {
                if (controller != null)
                {
                    controller.CollisionRadius = f;
                    controller.UpdateEars();
                }
            }));

            MakerAPI.AddControl(EnableWiggleToggle);
            MakerAPI.AddControl(DampingSlider);
            MakerAPI.AddControl(ElasticitySlider);
            MakerAPI.AddControl(StiffnessSlider);
            MakerAPI.AddControl(InertiaSlider);
            MakerAPI.AddControl(GravityXSlider);
            MakerAPI.AddControl(GravityYSlider);
            MakerAPI.AddControl(GravityZSlider);
            MakerAPI.AddControl(CollisionRadiusSlider);

            UpdatingGUI = false;
        }

    }
}
