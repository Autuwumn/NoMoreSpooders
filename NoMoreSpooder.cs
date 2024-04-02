using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExitGames.Client.Photon.StructWrapping;
using HarmonyLib;
using Photon.Pun;
using System.Buffers;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AssetBundlePatching;
using UnityEngine.Rendering;

namespace NoMoreSpooder
{
    [BepInPlugin(ModID, ModName, Version)]
    [BepInProcess("ContentWarning.exe")]
    public class NMS : BaseUnityPlugin
    {
        private const string ModID = "koala.contentwarning.nomorespooders";
        private const string ModName = "No More Spooders";
        private const string Version = "0.0.1";

        internal static AssetBundle Assets;

        public static NMS Instance { get; private set; }

        private void Awake()
        {
            var harmony = new Harmony(ModID);
            harmony.PatchAll();
            if (Instance == null) { Instance = this; } else { Destroy(this); }


            var bundleName = typeof(NMS).Assembly.GetManifestResourceNames().Single(str => str.EndsWith("nospooders"));
            var stream = typeof(NMS).Assembly.GetManifestResourceStream(bundleName);
            NMS.Assets = AssetBundle.LoadFromStream(stream);

        }
        public void Update()
        {
            foreach(var spi in FindObjectsOfType(typeof(SpiderContentProvider)))
            {
                var spo = spi as SpiderContentProvider;
                if (spo.transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh != NMS.Assets.LoadAsset<GameObject>("NewSpider").GetComponent<SkinnedMeshRenderer>().sharedMesh)
                {
                    spo.transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().rootBone.transform.localScale = Vector3.one * 25;
                    spo.transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh = NMS.Assets.LoadAsset<GameObject>("NewSpider").GetComponent<SkinnedMeshRenderer>().sharedMesh;
                }
            }
        }
    }
}
