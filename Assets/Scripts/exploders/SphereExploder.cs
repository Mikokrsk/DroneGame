using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class SphereExploder : Exploder
{
    [SerializeField] int _maxDepth;
    private float damage;
    /*    public override IEnumerator explode()
        {
            exploded = true;

            ExploderComponent[] components = GetComponents<ExploderComponent>();
            foreach (ExploderComponent component in components)
            {
                component.onExplosionStarted(this);
            }
    /*
            GetComponent<Collider>().isTrigger = true;
            Vector3 explosionPos = transform.position;
           // Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            var colliders = shootRay();
            foreach (Collider hit in colliders)
            {
                if (hit && hit.GetComponent<Rigidbody>())
                {
                    hit.GetComponent<Rigidbody>().AddExplosionForce(power, explosionPos, radius);
                }
    /*            else if (hit.GetComponent<Collider>().gameObject.name == "MainCamera")
                {
                    Vector3 dPos = Random.onUnitSphere * 0.01f;
                    hit.transform.position = hit.transform.position + dPos;
                }/*

            }
            GetComponent<Collider>().isTrigger = false;/*

          //  yield return new WaitForEndOfFrame();

                    while (true)
                    {
                       // disableCollider();
                        for (int i = 0; i < probeCount; i++)
                        {
                            shootFromCurrentPosition();
                        }
                      //  enableCollider();
                        yield return new WaitForFixedUpdate();
                    }
        }
    */

    public override IEnumerator explode()
    {
        ExploderComponent[] components = GetComponents<ExploderComponent>();
        foreach (ExploderComponent component in components)
        {
            if (component.enabled)
            {
                component.onExplosionStarted(this);
            }
        }
        for (int i = 0; i < probeCount; i++)
        {
            shootFromCurrentPosition();
        }
        yield return new WaitForFixedUpdate();
    }

    protected override void shootFromCurrentPosition()
    {
        Vector3 probeDir = Random.onUnitSphere;
        Ray testRay = new Ray(transform.position, probeDir);
        shootRay(testRay, radius, 0, _maxDepth);
    }


    private void shootRay(Ray testRay, float estimatedRadius, int depth = 0, int maxDepth = 10)
    {
        if (depth >= maxDepth)
        {
            return;
        }

        Debug.DrawRay(testRay.origin, testRay.direction * estimatedRadius, Color.green, 5f);
        RaycastHit hit;
        if (Physics.Raycast(testRay, out hit, estimatedRadius))
        {

            var healthController = hit.collider.GetComponent<HealthController>();

            if (healthController != null)
            {
                healthController.TakeDamage(damage);

            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(power * Time.deltaTime * testRay.direction / probeCount, hit.point);
                // estimatedRadius /= 2;
                var bombScript = hit.rigidbody.GetComponent<Bomb>();
                if (bombScript != null)
                {
                    bombScript.StartExplosion();
                }


            }
            /*            else
                        {
                            Vector3 reflectVec = Random.onUnitSphere;
                            if (Vector3.Dot(reflectVec, hit.normal) < 0)
                            {
                                reflectVec *= -1;
                            }
                            Ray emittedRay = new Ray(hit.point, reflectVec);
                            shootRay(emittedRay, estimatedRadius - hit.distance, depth + 1, maxDepth);
                        }*/
        }
        shootRay(testRay, estimatedRadius, depth + 1, maxDepth);
    }

    /*    private void Update()
        {
            if (explosionTime > 0)
            {
                explosionTime -= Time.deltaTime;
            }
        }*/

    public void StartExploded(float damage)
    {
        if (!exploded)
        {
            this.damage = damage;
            power *= 10000;
            exploded = true;
            explosionTime = 2;
            StartCoroutine("explode");
        }
    }
}
