using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Throw(Vector3 F)
    {
        GetComponent<Rigidbody>().AddForce(F);
        Invoke("Explode", 5);
    }

    public void Explode()
    {
        GameObject Planet = AssetManager.Instance.Get("Planet");
        //Remove from planet list
        Explosion = Instantiate(Explosion, Planet.transform, true);
        Explosion.transform.localPosition = transform.localPosition;
        Explosion.transform.localScale = Vector3.one * 0.3f;
        Destroy(Explosion, 1.2f);
        Planet.GetComponent<Planet>().RemoveRigidbody(GetComponent<Rigidbody>());
        Destroy(gameObject);
    }
}
