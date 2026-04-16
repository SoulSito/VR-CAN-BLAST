using UnityEngine;

public class GunSettings
{
    public int maxBullets = 7;
    public bool hasLaser = true;

    public GunSettings(
        int maxBullets = 7,
        bool hasLaser = true
    ) {
        this.maxBullets = maxBullets;
        this.hasLaser = hasLaser;
    }
}
