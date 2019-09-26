using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ChoppedBody : MonoBehaviour
{
    public Material         transparentMaterial;
    public Material         diffuseMaterial;
    public AgentType        agentType;
    public ChoppedBodyType1 choppedBodyType;

    Transform           _trans;
    Animation           _anims;
    AudioSource         _audio;
    SkinnedMeshRenderer _renderer;    

    void Awake()
    {
        _trans = transform;
        _anims = GetComponent<Animation>();
        _audio = GetComponent<AudioSource>();
        _renderer = GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        _anims[_anims.clip.name].wrapMode = WrapMode.ClampForever;
    }

    public void Activate(Transform spawnTransform)
    {
        _trans.position = spawnTransform.position;
        _trans.rotation = spawnTransform.rotation;
        //_anims.Play();
        Phenix.Unity.Utilities.Tools.PlayAnimation(_anims, _anims.clip.name, 0.5f);
        _audio.Play();
        _renderer.material = diffuseMaterial;
        StartCoroutine(Fadeout(10, 3));
        //GuiManager.Instance.ShowBloodSplash();
    }

    public void Deactivate()
    {

    }

    protected IEnumerator Fadeout(float delay, float disappearTime)
    {
        if (transparentMaterial == null)
            yield break;

        yield return new WaitForSeconds(.1f);
        //SpriteEffectsManager.Instance.CreateBloodSlatter(Transform, 2, 3);
        SpriteMgrComponent.Instance.CreateSprite(SpriteType.BLOOD_BIG, 
            new Vector3(_trans.localPosition.x, _trans.localPosition.y + 0.5f, _trans.localPosition.z),
            new Vector3(90, Random.Range(0, 180), 0));
        yield return new WaitForSeconds(delay);
        //CombatEffectsManager.Instance.PlayDisappearEffect(Transform.position, Transform.forward);
        _renderer.material = transparentMaterial;
                
        transparentMaterial.SetColor("_Color", Color.white);
        Color finalColor = new Color(1, 1, 1, 0);
        float passTime = 0;
        while (passTime < disappearTime)
        {
            passTime += Time.deltaTime;
            //color.a = Mathf.Lerp(1, 0, passTime / 5);
            //transparentMaterial.SetColor("_Color", color);
            transparentMaterial.SetColor("_Color", Color.Lerp(Color.white, finalColor, passTime/disappearTime));
            yield return new WaitForEndOfFrame();
        }
        /*while (color.a > 0)
        {
            color.a -= Time.deltaTime;
            if (color.a < 0)
                color.a = 0;

            transparentMaterial.SetColor("_Color", color);
            yield return new WaitForEndOfFrame();
        }*/

        ChoppedBodyMgr1.Instance.Collect(gameObject, agentType, choppedBodyType);
    }

}



