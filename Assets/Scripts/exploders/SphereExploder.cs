using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class SphereExploder : Exploder
{
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
        while (explosionTime > 0)
        {
            disableCollider();
            for (int i = 0; i < probeCount; i++)
            {
                shootFromCurrentPosition();
            }
            enableCollider();
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void shootFromCurrentPosition()
    {
        Vector3 probeDir = Random.onUnitSphere;
        Ray testRay = new Ray(transform.position, probeDir);
        shootRay(testRay, radius);
    }


    private void shootRay(Ray testRay, float estimatedRadius, int depth = 0, int maxDepth = 10)
    {
        if (depth >= maxDepth)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(testRay, out hit, estimatedRadius))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(power * Time.deltaTime * testRay.direction / probeCount, hit.point);
                estimatedRadius /= 2;
            }
            else
            {
                Vector3 reflectVec = Random.onUnitSphere;
                if (Vector3.Dot(reflectVec, hit.normal) < 0)
                {
                    reflectVec *= -1;
                }
                Ray emittedRay = new Ray(hit.point, reflectVec);
                shootRay(emittedRay, estimatedRadius - hit.distance, depth + 1, maxDepth);
            }
        }
    }

    private void Update()
    {
        if (explosionTime > 0)
        {
            explosionTime -= Time.deltaTime;
        }
    }

    public void StartExploded()
    {
        if (!exploded)
        {
            power *= 10000;
            exploded = true;
            explosionTime = 2;
            StartCoroutine("explode");
        }
    }
}
