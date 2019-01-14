using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BlackGhoulBattle : BasicEnemyBehaviour
{
    //Note: On my edits I am assuming both the darkpulse and the normal attack will be used. If this changes edits will need to be made

    //Need to mess around with this, make Darkpulse on of the i's
    private int i; // 0 = BombAttack, 1 = LaserAttack;

    //bool for coroutines

    private bool Bomb; //Remove later
    private bool Laser; //Remove Later
    private bool Pulse; //Bool for the dark Pulse attack

    //remove/replace later as necessary
    private GameObject meteorLaunchAnimation; 
    private GameObject laserGathering; 
    private GameObject laserWarningGameObject; 
    private GameObject Warning; 
    private GameObject[] WarningMeteor;
    private GameObject PulseAreaWarningAnimation;
    private GameObject PulsePathGameObject;
    //Don't think I actually want these as they are
    private GameObject PulseShot;
    private GameObject DarkPulse;

    //Will need to remake this for the ghoul at some point. Maybe not, just change the gameobjects placed in them
    public Slider _CastSpellSlider;
    public GameObject _CastSpellGameobject;
    public Text _SpellCasttimer;

    //Probably just apply the same stun as the third enemy
    public Slider _StunnedSlider;
    public GameObject _StunnedGameObject;
    public Text _StunnedTimer;
    private bool isStunned;

    //Leave this as well for stuns
    private float StunDurationTime;
    private float StunActualtime;

    //Not sure what to do about this? Likely need to remake it as well?
    public float LineDrawSpeed;

    //Lots here will need to be remove but some will need to be reused
    private float m_warningCastTime;
    public GameObject LaserBeamHit;
    public bool m_warningCastTimeBool;
    private Vector3 m_targetPos;
    private bool m_dashingAnim;
    private bool laser;
    private LineRenderer laserWarning;
    private bool laserWarningBool;
    public int Aggro;
    public bool StopAttacking;
    private float laserCounter;
    private float laserWarningCounter;
    private float laserDist;
    private LineRenderer PulsePath;

    //Keep this, change name
    [Header("SpellsPrefab (normal, darkpulse/bomb, //laser)")]
    public GameObject[] _AttackPrefabs;

    //Need to edit to make dark pulse one. I want dark pulse to be AttackCD[1]
    [Header("Attack Cooldowns (normal, darkpulse/bomb, //laser)")]
    public float[] _AttackCD;

    //Need to edit to include dark pulse attack and remove others
    [Header("Attack Cast Time (darkpulse/bomb, //laser)")]
    public float[] _TimeWarningForSpell; // i = 0 is BombAttack, i = 1 is Laser;

    //Keep these, change name
    [Header("Attack Warning FX (darkpulse/bomb, //laser)")]
    public GameObject[] _AttackWarningPrefabs;
    [Header("Attack Animation FX (darkpulse/bomb, //laser)")]
    public GameObject[] _AttackAnimations;

    //Keep this, change name and include darkpulse
    [Header("AttackDamages (normal, darkpulse/bomb, //laser)")]
    public int[] _AttackDamage;

    //Can maybe remove this? check it out first
    [Header("BasicSpellData")]
    public float TimeBetweenMeteors = 2;
    public int NumberOfMeteors = 4;

    //Figure out what this is before deleting it
    private GameObject[] Attack;
    private GameObject[] AttackFx;
    public LineRenderer[] LaserBeam;
    private NavMeshAgent meshAgent;

    [SerializeField] private float m_darkpulseTimer;
    [SerializeField] private float m_normalAttackTimer;
    //remove all these later
    [SerializeField] private float m_bombAttackTimer;
    [SerializeField] private float m_laserAttackTimer;

    new void Start()
    {
        base.Start();

        //Figure out what this is and determine whether to keep or not
        for (int i = 0; i < LaserBeam.Length; i++)
        {
            LaserBeam[i].sortingOrder = 10;
            LaserBeam[i].SetPosition(0, transform.position);
        }

        m_nav = GetComponent<NavMeshAgent>();
        _BoyOrGirl = Random.Range(0, 2); //Setting target?
    }

    new void Update()
    {
        base.Update();

        //Will want to keep and use this for the projectile's path
        if (laserWarningBool)
        {
            transform.LookAt(_Target[_BoyOrGirl]);

            laserWarning.SetPosition(0, transform.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                laserDist = Vector3.Distance(transform.position, hit.point);

                if (hit.collider && laserWarningCounter < laserDist)
                {
                    laserWarningCounter += .1f / LineDrawSpeed;

                    float x = Mathf.Lerp(0, laserDist, laserWarningCounter);

                    Vector3 pointA = transform.position;
                    Vector3 pointB = hit.point;

                    Vector3 pointAlLongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                    laserWarning.SetPosition(1, pointAlLongLine);
                }
                else if (hit.collider && laserWarningCounter >= laserDist)
                {
                    laserWarning.SetPosition(1, hit.point);
                }
            }
        }

        //Want to keep this for spell casting warnings
        if (m_warningCastTimeBool == true)
        {
            if (m_warningCastTime < _TimeWarningForSpell[i])
            {
                m_warningCastTime += Time.deltaTime;
            }
            _CastSpellSlider.value = m_warningCastTime / _TimeWarningForSpell[i];
            _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i] - m_warningCastTime), 2).ToString();
        }

        //Keep this for stun mechanic
        if (isStunned == true)
        {
            if (StunActualtime >= 0)
            {
                StunActualtime -= Time.deltaTime;
            }
            _StunnedSlider.value = StunActualtime / StunDurationTime;
            _StunnedTimer.text = System.Math.Round((float)(StunActualtime), 2).ToString();
        }

        //Will need to keep this
        //Set up this way so that when the Coroutine starts, it isn't reset on the next frame.
        //StopAttacking is dealt with within the Coroutine.
        //The coroutine doesn't override the update function so the coroutine would be called continuously without this
        if (!StopAttacking)
        {
            m_normalAttackTimer += Time.deltaTime;
            m_darkpulseTimer += Time.deltaTime;
            //remove these
            m_bombAttackTimer += Time.deltaTime;
            m_laserAttackTimer += Time.deltaTime;

            NormalAttack();
            DarkPulseAttack();
            //Remove these
            BombAttack();
            LaserAttack();
        }

        //Most likely want to keep this but do some major edits to make it the path of the dark pulse projectile instead of a laser
        //May not need to be in update if the projectile and or warning area don't change according to the player's movement
        if (laser)
        {
            transform.LookAt(_Target[_BoyOrGirl]);
            for (int i = 0; i < LaserBeam.Length; i++)
            {
                LaserBeam[i].SetPosition(0, transform.position);
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider)
                {
                    laserDist = Vector3.Distance(transform.position, hit.point);

                    if (LaserBeamHit && laserCounter < laserDist)
                    {
                        laserCounter += .1f / LineDrawSpeed;

                        float x = Mathf.Lerp(0, laserDist, laserCounter);

                        Vector3 pointA = transform.position;
                        Vector3 pointB = hit.point;

                        Vector3 pointAlLongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

                        for (int i = 0; i < LaserBeam.Length; i++)
                        {
                            LaserBeam[i].SetPosition(1, pointAlLongLine);
                        }
                        LaserBeamHit.transform.position = pointAlLongLine;
                    }
                    else
                    {
                        for (int i = 0; i < LaserBeam.Length; i++)
                        {
                            LaserBeam[i].SetPosition(1, hit.collider.transform.position);
                            LaserBeamHit.transform.position = hit.point;

                        }
                    }

                    if (hit.collider.tag == "Player")
                    {
                        MessageHandler msgHandler = hit.collider.GetComponent<MessageHandler>();
                        DamageData dmgData = new DamageData();
                        dmgData.damage = _AttackDamage[2];
                        if (msgHandler)
                        {
                            msgHandler.GiveMessage(MessageTypes.DAMAGED, this.gameObject, dmgData);
                            GameObject go = hit.collider.GetComponent<HealthController>().Sprite;
                            Canvas[] canvas = go.GetComponentsInChildren<Canvas>();

                            for (int i = 0; i < canvas.Length; i++)
                            {
                                if (canvas[i].GetComponentInChildren<DamageDisplayScript>())
                                    canvas[i].GetComponentInChildren<DamageDisplayScript>().GetDamageText(Color.red, _AttackDamage[2]);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < LaserBeam.Length; i++)
                    {
                        LaserBeam[i].SetPosition(1, transform.forward * 5000);
                    }
                }
            }
            CancelAttack();
        }

        //Implementing to move towards and follow target
        if (!isCollided)
        {
            if (i == 1 && _Target[_BoyOrGirl].GetComponent<HealthController>().CurrentHealth == 0)
            {
                i = 0;
            }
            else if (i == 0 && _Target[_BoyOrGirl].GetComponent<HealthController>().CurrentHealth == 0)
            {
                i = 1;
            }
            if (_Target[_BoyOrGirl])
            {
                m_nav.SetDestination(_Target[_BoyOrGirl].transform.position);
            }

        }
    }

    private void NormalAttack()
    {
        if (m_normalAttackTimer >= _AttackCD[0])
        {
            CastNormalAttack();
            m_normalAttackTimer = 0;
        }
    }

    private void DarkPulseAttack()
    {
        if(m_darkpulseTimer >= _AttackCD[1])
        {
            StartCoroutine("CastDarkPulseAttack");
            m_darkpulseTimer = 0;
        }
    }

    //Remove
    private void BombAttack()
    {
        if (m_bombAttackTimer >= _AttackCD[1])
        {
            StartCoroutine("CastBombAttack");
        }
    }

    //Remove
    private void LaserAttack()
    {
        if (m_laserAttackTimer >= _AttackCD[2])
        {
            StartCoroutine("CastLaserAttack");
        }
    }

    //Definitely keep this. This ends attacks when the boss/enemy is dead
    private void CancelAttack()
    {
        if (gameObject.GetComponent<HealthController>().CurrentHealth == 0)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator CastDarkPulseAttack()
    {
        StopAttacking = true; //To stop the function in the update
        Pulse = true; //This bool is used in the stun function to check if the spell is currently happening
        i = 1; //Not sure this is the number I want (currently the same as laser, should sort it out)
        _BoyOrGirl = Random.Range(0, 2); //Selecting a target

        //Need to move to target if target is not in range here

        //Notes: Want to draw a line similarly to the laser, then cast the spell trajectory, then cause the explosion/damage

        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        //Need to put a warning here and a yield function.
        //Need to think of specifics: do I want to give the player a warning of who is going to be attacked? Probably
        //How do I provide that warning? With the darkpulse area warning

        //Initial yield before warning starts
        yield return new WaitForSeconds(2);

        //Instantiating warning
        //What is the tranform in the next line? Should I change it to something like "target.transform.position.x"
        Vector3 PulseLaunch = _Target[_BoyOrGirl].transform.position - transform.position; //determining the direction of attack
        float angle = Mathf.Atan2(PulseLaunch.z, PulseLaunch.x) * Mathf.Rad2Deg; //translating the direction into an angle(in degrees)
        PulseAreaWarningAnimation = Instantiate(_AttackAnimations[0]/*Need to makre sure this is input right later*/, PulseLaunch, Quaternion.identity);

        //The rest of the wait time adjusted for previous pause
        yield return new WaitForSeconds(_TimeWarningForSpell[i] - 2);


        transform.LookAt(_Target[_BoyOrGirl].transform); //looking at target?

        int rotation = 0; //Making the rotation 0

        //the following ifs give a value to the variable "rotation" if the target is in that direction
        if (45 <= transform.eulerAngles.y && transform.eulerAngles.y < 135)
        {
            rotation = 3;
        }
        else if (0 <= transform.eulerAngles.y && transform.eulerAngles.y < 45)
        {
            rotation = 1;
        }
        else if (225 <= transform.eulerAngles.y && transform.eulerAngles.y < 315)
        {
            rotation = 4;
        }
        else
        {
            rotation = 2;
        }

        if (rotation != 0) //rotating the sprite
        {
            _Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }

        //This is getting rid of the casting warning because the spell is about to start
        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //destroy dark pulse area warning at the same time the path is destroyed
        Destroy(PulseAreaWarningAnimation);
        //Instantiate then destroy dark pulse path
        GameObject go = Instantiate(_AttackPrefabs[0], transform.position, Quaternion.Euler(0, -angle, 0)); //instantiate the game object
        go.gameObject.GetComponent<Spell>().SetTarget(_Target[_BoyOrGirl].gameObject); //sets the target
        go.gameObject.GetComponent<Bullet>().SpellFlare(angle); //using bullet script of angle
        go.gameObject.GetComponent<Bullet>().SetSpellCaster(this.gameObject); //setting spellcaster in bullet script

        //Need to instantiate and destroy the actual dark pulse explosion

        //End stuff
        _BoyOrGirl = Random.Range(0, 2); //randomises the target
        m_darkpulseTimer = 0; //resetting the timer
        Destroy(DarkPulse); //destroying the darkpulse animation
        Pulse = false; //restting the Pulse bool
        StopAttacking = false; //allows the timers to start counting again. Very important that this is the very last thing in the coroutine
    }

    //Don't want to keep this, but there is a lot I'll need to use for the dark pulse in here
    private IEnumerator CastBombAttack() //Work in Prgress
    {
        StopAttacking = true; //To stop the function in the update
        Bomb = true; //This bool is used in the stun function to check if the spell is currently happening
        i = 0; //This is using the variable from earlier
        _BoyOrGirl = Random.Range(0, 2); //selecting a random target

        //Look toward target and warn line

        //Slider; Will probably want something similar for the dark pulse. This is setting up a warning timer
        m_warningCastTime = 0;
        _CastSpellGameobject.SetActive(true);
        _SpellCasttimer.enabled = true;
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString();
        m_warningCastTimeBool = true;

        //I think this is the timing before the warning circles pop up
        yield return new WaitForSeconds(2);

        //Selecting the position that the meteor will land on
        Vector3 meteorLaunch = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1.9f); //Not super sure what the z adjustment is for, may need to edit it for the dungeon floor
        meteorLaunchAnimation = Instantiate(_AttackAnimations[0], meteorLaunch, Quaternion.identity);

        //This is the rest of the warning time, eddited to account for the previous yield function
        yield return new WaitForSeconds(_TimeWarningForSpell[i] - 2);

        //This is getting rid of the casting warning because the spell is about to start
        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //Start the Attack
        //I'm not entirely sure what this is doing, but I think I'll need it for the dark pulse, except 1 instead of 3
        Vector3[] MeteorTargetpos = new Vector3[NumberOfMeteors];
        GameObject[] meteors = new GameObject[NumberOfMeteors];
        WarningMeteor = new GameObject[NumberOfMeteors];

        //This is creating the meteors and destroying the warning circles
        for (int i = 0; i < meteors.Length; i++)
        {
            MeteorTargetpos[i] = _Target[_BoyOrGirl].transform.position; //selecting where the warning circle will go
            WarningMeteor[i] = Instantiate(_AttackWarningPrefabs[0], MeteorTargetpos[i], Quaternion.identity); //instantiating the warning circle
            Destroy(WarningMeteor[i], 1.5f + TimeBetweenMeteors); //Setting the circle to be destroyed in a certain amout of time
            yield return new WaitForSeconds(TimeBetweenMeteors); //waiting
            meteors[i] = Instantiate(_AttackPrefabs[1], MeteorTargetpos[i], Quaternion.identity); //instantiating the actual meteor
            meteors[i].GetComponent<Bullet>().Damage = _AttackDamage[1]; //doing damage
            yield return new WaitForSeconds(TimeBetweenMeteors + 0.5f); //waiting for some reason
        }

        //Will need similar end stuff for the dark pulse
        m_bombAttackTimer = 0; //resetting the timer
        Destroy(meteorLaunchAnimation); //destroying something
        Bomb = false; //restting the bomb bool
        StopAttacking = false; //allows the timers to start counting again. Very important that this is the very last thing in the coroutine
    }

    private IEnumerator CastLaserAttack()
    {
        StopAttacking = true; //Stops the timers in the update function
        Laser = true; //Used in the stun function
        i = 1; //not sure if I want to make dark pulse i=1 or i=0
        _BoyOrGirl = Random.Range(0, 2); //Selecting a random target

        //Look toward target and warn line

        //Slider
        m_warningCastTime = 0; //reset the warning time; not sure why this seems to be communal
        _CastSpellGameobject.SetActive(true); //instantiate a CastSpell gameobject
        _SpellCasttimer.enabled = true; //enable the timer; again communal
        _SpellCasttimer.text = System.Math.Round((float)(_TimeWarningForSpell[i]), 2).ToString(); //setting the slider
        m_warningCastTimeBool = true; //?? Starting the warning?

        //The warning time before the real warnings start?
        yield return new WaitForSeconds(1f);

        //Starting the animation to gather the energy before releasing the laser
        laserGathering = Instantiate(_AttackAnimations[1], transform.position, Quaternion.identity);

        //????
        laserWarningBool = true;

        laserWarningGameObject = Instantiate(_AttackWarningPrefabs[1], transform.position, Quaternion.identity); //Instantiating a warning prefab
        laserWarning = laserWarningGameObject.GetComponent<LineRenderer>(); //getting a line renderer
        laserWarning.sortingOrder = 10; //No fucking clue what the hell this is

        //Last bit of warning, acconting for previous bit by subtracting 2
        yield return new WaitForSeconds(_TimeWarningForSpell[i] - 1f);

        //Destroying the laser gathering animation
        Destroy(laserGathering, 2f);

        //Destroying slider and other stuff used in warning
        m_warningCastTimeBool = false;
        _SpellCasttimer.enabled = false;
        _CastSpellGameobject.SetActive(false);

        //Start the Attack

        laser = true; //turns on the laser function in the update that emits particles
        LaserBeamHit.SetActive(true); //A gameobject for when the laser hits something; turned off in stun if on

        //????
        yield return new WaitForEndOfFrame();

        //Destroying some laser warnings here?  why? Hasn't the attack started?
        laserWarningBool = false;
        Destroy(laserWarningGameObject);

        //Playing the attack animation
        _AttackAnimations[2].GetComponent<ParticleSystem>().Play();

        //Enables the laser up to it's max length
        for (int i = 0; i < LaserBeam.Length; i++)
        {
            LaserBeam[i].enabled = true;
        }

        //The duration of the laser attack I believe
        yield return new WaitForSeconds(5);

        //Turns off the laser
        for (int i = 0; i < LaserBeam.Length; i++)
        {
            LaserBeam[i].enabled = false;
        }

        LaserBeamHit.SetActive(false); //Turns off laser beam hit
        laser = false; //turns off laser function in update

        //Ends the attack animation
        _AttackAnimations[2].GetComponent<ParticleSystem>().Stop();

        //Resets at the end
        m_laserAttackTimer = 0; //resets timer
        laserCounter = 0; //Resets a counter??
        laserWarningCounter = 0; //resets another counter?
        Laser = false; //turns off laser bool for stun function
        StopAttacking = false; //Allows update to count cooldowns again, important that this is the last thing
    }

    //Should want to essentially keep all of this
    private void CastNormalAttack()
    {
        Vector3 direction = _Target[_BoyOrGirl].transform.position - transform.position; //determining the direction of attack
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg; //translating the direction into an angle(in degrees)

        transform.LookAt(_Target[_BoyOrGirl].transform); //looking at target?

        int rotation = 0; //Making the rotation 0

        //the following ifs give a value to the variable "rotation" if the target is in that direction
        if (45 <= transform.eulerAngles.y && transform.eulerAngles.y < 135)
        {
            rotation = 3;
        }
        else if (0 <= transform.eulerAngles.y && transform.eulerAngles.y < 45)
        {
            rotation = 1;
        }
        else if (225 <= transform.eulerAngles.y && transform.eulerAngles.y < 315)
        {
            rotation = 4;
        }
        else
        {
            rotation = 2;
        }

        if (rotation != 0) //rotating the sprite
        {
            _Sprite.GetComponent<SpriteScript2>().ForcePlayerRotation(rotation);
        }

        GameObject go = Instantiate(_AttackPrefabs[0], transform.position, Quaternion.Euler(0, -angle, 0)); //instantiate the game object
        go.gameObject.GetComponent<Spell>().SetTarget(_Target[_BoyOrGirl].gameObject); //sets the target
        //go.gameObject.GetComponent<Bullet>().GetAggro(Aggro); //Maybe used later for aggro
        go.gameObject.GetComponent<Bullet>().SpellFlare(angle); //using bullet script of angle
        go.gameObject.GetComponent<Bullet>().SetSpellCaster(this.gameObject); //setting spellcaster in bullet script
        _BoyOrGirl = Random.Range(0, 2); //randomises the target
    } //Finished

    //Not Used, have to keep for basic enemy behaviour
    public override void ResetToDefaults()
    {
        throw new System.NotImplementedException();
    }

    public override void Stunned(GameObject StunAnim, float StunDuration)
    {
        //setting variables
        StunDurationTime = StunDuration; 
        StunActualtime = StunDuration;

        if (Bomb) //if in the bomb coroutine
        {
            StopCoroutine("CastBombAttack"); //stop the coroutine

            if (meteorLaunchAnimation) //if the animation is happening, destroy it
                Destroy(meteorLaunchAnimation);

            m_bombAttackTimer = 0; //reset the timer so it doesn't happen immediately again

        }

        if (Laser) //if in the laser coroutine:
        {
            StopCoroutine("CastLaserAttack"); //stop the coroutine

            if (laserWarningBool) //stop the laserwarning function in update
                laserWarningBool = false;
            if (laser) //stops the laser function in update
                laser = false;
            if (LaserBeamHit.activeSelf == true) //turns off the laserbeamhit animation
                LaserBeamHit.SetActive(false);
            for (int i = 0; i < LaserBeam.Length; i++) //destroys laser beam
            {
                if (LaserBeam[i].enabled)
                    LaserBeam[i].enabled = false;
            }

            if (laserGathering) //destroys laser gathering animation
                Destroy(laserGathering);
            if (laserWarningGameObject) //destroys laser warning
                Destroy(laserWarningGameObject);
 
            laserCounter = 0; //resets the laser counter
            laserWarningCounter = 0; //resets the laser warning counter
            if (_AttackAnimations[2].GetComponent<ParticleSystem>().isPlaying == true) //ends the attack animation
                _AttackAnimations[2].GetComponent<ParticleSystem>().Stop();

            m_laserAttackTimer = 0; //resets the laser attack timer
        }

        if (m_warningCastTimeBool) //resets the bool
            m_warningCastTimeBool = false;
        if (_SpellCasttimer.enabled) //resets the spellcasttimer
            _SpellCasttimer.enabled = false;
        if (_CastSpellGameobject.activeSelf == true) //resets the castspell gameobject
            _CastSpellGameobject.SetActive(false);

        StartCoroutine(StunCoroutine(StunAnim)); //Starts the stun coroutine
    }

    private IEnumerator StunCoroutine(GameObject Stun)
    {
        _StunnedGameObject.SetActive(true); //stuns gameobject
        _StunnedTimer.enabled = true; //enables stun timer

        isStunned = true; //is stunned variable = true; starts counting the stun timer in update

        Debug.Log("Stun start");
        GameObject StunAnim = Instantiate(Stun, transform.position + new Vector3(0, 0, 1.4f), Quaternion.Euler(0, 35, 0));//Instantiate the stun animation
        yield return new WaitForSeconds(StunDurationTime);//waiting for the duration of the stun
        StunAnim.GetComponent<ParticleSystem>().Stop();// stop the stun animation
        Destroy(StunAnim, 1f);//destroy the stun animation
        Debug.Log("Stun end");
        StopAttacking = false;//ends the stopattacking bool to allow timers to count
        isStunned = false;//make isstunned false and stop the function in update

        //resetting at the end
        _StunnedGameObject.SetActive(false);
        _StunnedTimer.enabled = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (_Target[_BoyOrGirl])
        {
            if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
            {
                CancelGhoulMovement();
            }
            else return;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        if (_Target[_BoyOrGirl])
        {
            if (other == _Target[_BoyOrGirl].GetComponent<Collider>())
            {
                isCollided = false;
                //will have player chase target once target leaves attack range trigger
            }
        }
    }

    private void CancelGhoulMovement()
    {
        isCollided = true;
        m_nav.SetDestination(m_nav.transform.position);
    }
}


