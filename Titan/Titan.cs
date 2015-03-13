using ICities;
using UnityEngine;

namespace Titan
{
    public class Titan : IUserMod
    {
        public string Name { 
            get 
            {
                GameObject go = new GameObject("Titan Industries");
                go.AddComponent<TitanCore>();
                return "Titan Industries"; 
            } 
        }
        public string Description { get { return "The Titan mod introduces a range of new features to the game."; } }
    }

    //public class LoadingExtension : LoadingExtensionBase
    //{
    //    private GameObject _gameObject;

    //    public override void OnLevelLoaded(LoadMode mode)
    //    {
    //        if(_gameObject == null)
    //        {
    //            _gameObject = new GameObject();
    //            _gameObject.AddComponent<TitanCore>();
    //        }
    //    }

    //    public override void OnLevelUnloading()
    //    {
    //        if(_gameObject != null)
    //        {
    //            GameObject.Destroy(_gameObject);
    //            _gameObject = null;
    //        }
    //    }
    //}
}
