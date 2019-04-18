using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private static float RANGE = 3f;
    private static float DAMAGE = 30f;
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
        Explosion = Instantiate(Explosion, Planet.transform, true);
        Explosion.transform.localPosition = transform.localPosition;
        Explosion.transform.localScale = Vector3.one * 0.3f;
        Destroy(Explosion, 1.2f);
        //Remove from planet list
        Planet.GetComponent<Planet>().RemoveRigidbody(GetComponent<Rigidbody>());
        //Damage to players
        PlayerController[] Players = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnController>().GetPlayers();
        foreach(PlayerController p in Players)
        {
            float distance = Vector3.Distance(p.transform.position, transform.position);
            float damage = Mathf.Max(0,DAMAGE * (1 - distance / RANGE));
            p.CmdTakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
