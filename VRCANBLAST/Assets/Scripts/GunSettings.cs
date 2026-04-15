using UnityEngine;

public class GunSettings
{
    public int maxBullets = 3;
    public bool hasLaser = false;

    public GunSettings(
        int maxBullets = 3,
        bool hasLaser = false
    ) {
        this.maxBullets = maxBullets;
        this.hasLaser = hasLaser;
    }
}
