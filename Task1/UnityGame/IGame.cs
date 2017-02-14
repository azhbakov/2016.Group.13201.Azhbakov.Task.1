using System.Collections.Generic;

namespace Task1.UnityGame {
    public interface IGame {
        GameObject Instantiate (float x = 0f, float y = 0f);
        GameObject FindObjectWithTag (string tag);
        List<GameObject> FindObjectsWithTag (string tag);
        TComponentType FindComponentByTag<TComponentType> (string tag) where TComponentType : class, IComponent;
        Input Input ();
    }
}
