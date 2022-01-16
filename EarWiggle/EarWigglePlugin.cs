using AIChara;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI;
using KKAPI.Chara;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EarWiggle
{
    [BepInPlugin(GUID, Name, Version)]
    [BepInDependency(KoikatuAPI.GUID, KoikatuAPI.VersionConst)]
    public class EarWigglePlugin : BaseUnityPlugin
    {
        public const string GUID = "orange.spork.earwiggle";
        public const string Name = "Ear Wiggle";
        public const string Version = "1.0.1";

        internal ManualLogSource Log => Logger;

        public static EarWigglePlugin Instance { get; set; }

        public EarWiggleMakerGUI MakerGUI { get; set; }

        public EarWigglePlugin()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Singleton only.");
            }

            Instance = this;

        }

        public void Start()
        {
            CharacterApi.RegisterExtraBehaviour<EarWiggleCharaController>(GUID);
            MakerGUI = new EarWiggleMakerGUI();
            MakerGUI.RegisterMakerAPIControls();
#if DEBUG
            Log.LogInfo($"Ear Wiggle Plugin Started");
#endif
        }

        public void Update()
        {

        }
    }
}
