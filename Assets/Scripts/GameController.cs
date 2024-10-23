using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using TMPro;
public class GameController : MonoBehaviour
{
    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    Pooler pooler;
    UIManager uIManager;

    public Material sliceMaterial;
    public TextureRegion textureRegion;

    public List<string> CountdownElements;
    public TextMeshPro CountdownText;
    SoundManager soundManager;
    private void Start()
    {
        pooler = Pooler.instance;
        uIManager = UIManager.instance;
        CountdownText.gameObject.SetActive(false);
        soundManager = SoundManager.instance;
    }
    IEnumerator Countdown(GameObject _Fruit)
    {
  
        string parentName =  _Fruit.transform.parent.name;
        if (parentName != "MainMenu")
        {
            CountdownText.gameObject.SetActive(true);
            for (int i = 0; i < CountdownElements.Count; i++)
            {
                CountdownText.text = CountdownElements[i];
                yield return new WaitForSeconds(1f);
            }
            CountdownText.gameObject.SetActive(false);
        }
        uIManager.SelectButton(parentName);
    }

    public void Slice(GameObject _Fruit, Weapon weapon)
    {
        SlicedHull slicedObject = Sliceed(_Fruit, weapon.slicePanel, sliceMaterial);

        if (slicedObject != null)
        {
            if (_Fruit.CompareTag("fruit"))
            {
                _Fruit.SetActive(false);
                Fruit fruit = _Fruit.GetComponent<Fruit>();
                pooler.ReycleFruit(fruit);
                if (fruit.particleTyp == particleType.Explosion)
                {
                    soundManager.BombSound();
                    uIManager.DecreaseHealth();
                }
                else if (fruit.particleTyp == particleType.Ice)
                {
                    soundManager.FrozenSound();
                    uIManager.IncreaseScore(4);
                }
                else
                {
                    soundManager.SplashFruit();
                    uIManager.IncreaseScore(1);
                }
            }
            else if (_Fruit.CompareTag("button"))
            {
                uIManager.BeforeSelectButton();
                soundManager.SplashFruit();
                StartCoroutine(Countdown(_Fruit));
            }
            pooler.GetParticle(_Fruit.GetComponent<Fruit>().particleTyp, _Fruit.transform.position, _Fruit.transform.rotation);
            GameObject upperPart = slicedObject.CreateUpperHull(_Fruit);
            GameObject lowPart = slicedObject.CreateLowerHull(_Fruit);
            AddComponents(upperPart);
            AddComponents(lowPart);
        }
    }

    public SlicedHull Sliceed(GameObject objectToSlice, GameObject plane, Material material)
    {
        return objectToSlice.Slice(plane.transform.position, plane.transform.up, material);

    }

    public void AddComponents(GameObject objPart)
    {

        objPart.AddComponent<BoxCollider>();
        objPart.GetComponent<MeshRenderer>().materials[1] = sliceMaterial;

        objPart.AddComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        objPart.GetComponent<Rigidbody>().AddExplosionForce(350, objPart.transform.position, 30);
        Destroy(objPart, 8f);
    }


}
