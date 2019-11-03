using UnityEngine;
using System.Collections;
using Phenix.Unity.UI;
using UnityEngine.UI;

public class testPageFlip : MonoBehaviour
{
    public FlipPage fp;

    // Use this for initialization
    void Start()
    {
        fp.onPageShow.AddListener(OnPageShow);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPageShow(int pageID, Image image)
    {
        if (image.transform.childCount > 0)
        {
            return;
        }
        GameObject child = new GameObject("buttonObj");
        child.AddComponent<Text>().text = "page " + pageID;        
        (child.transform as RectTransform).localPosition = Vector3.zero;
        child.transform.parent = image.transform;
    }
}
