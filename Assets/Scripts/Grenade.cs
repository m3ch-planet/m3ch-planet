using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private static float RANGE = 2f;
    private static float DAMAGE = 40f;
    private static float DELAY = 3;
    [SerializeField]
    GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Throw(Vector3 F)
    {
        GetComponent<Rigidbody>().AddForce(F);
        Invoke("Explode", DELAY);
    }

    public void Explode()
    {
        GameObject TC = GameObject.FindGameObjectWithTag("TurnController");
        GameObject Planet = AssetManager.Instance.Get("Planet");
        Explosion = Instantiate(Explosion, Planet.transform, true);
        Explosion.transform.localPosition = transform.localPosition;
        Explosion.transform.localScale = Vector3.one * 0.3f;
        Destroy(Explosion, 1.2f);
        //Remove from planet list
        Planet.GetComponent<Planet>().RemoveRigidbody(GetComponent<Rigidbody>());
        //Damage to players
        PlayerController[] Players = TC.GetComponent<TurnController>().GetPlayers();
        foreach(PlayerController p in Players)
        {
            float distance = Vector3.Distance(p.transform.position, transform.position);
            float damage = Mathf.Max(0,DAMAGE * (1 - distance / RANGE));
            p.CmdTakeDamage(damage);
        }
        TC.GetComponent<TurnController>().EndTurn();
        Destroy(gameObject);
    }
}
