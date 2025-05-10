public static class Enums
{
    public enum MaterialTypes
    {
        Road,
        Dirt,
        Grass,
        Carpet,
        Lam,
        Wood,
        Stone,
        Tile,
        Tarmac,
        Mesh,
        Metal,
        Rusty,
        Squelch
    }

    public enum ItemTypes
    {
        Melee,
        Gun,
        Supply,
        Key
    }

    public enum WeaponTypes
    {
        Fists,
        Bat,
        Bulk,
        Hammer,
        Knife,
        Spear,
        Sword,
        Pistol,
        Shotgun,
        Rifle,
    }

    public enum MovementStates
    {
        Idle,
        Walk,
        RunExhaust,
        Run
    }

    public enum LightMultiplier
    {
        a = 0,
        b = 8,
        c = 17,
        d = 25,
        e = 33,
        f = 42,
        g = 50,
        h = 58,
        i = 67,
        j = 75,
        k = 89,
        l = 92,
        m = 100,
        n = 107,
        o = 114,
        p = 121,
        q = 129,
        r = 143,
        s = 150,
        t = 157,
        u = 164,
        v = 171,
        w = 179,
        x = 186,
        y = 193,
        z = 200
    }

    public enum LightFlickerHz
    {
        OnePerSecond = 1,
        TwoPerSecond = 2,
        ThreePerSecond = 3,
        FivePerSecond = 5,
        TenPerSecond = 10,
        FifteenPerSecond = 15,
        TwentyPerSecond = 20,
        TwentyFivePerSecond = 25,
        ThirtyPerSecond = 30,
        FourteenFivePerSecond = 45,
        SixteenPerSecond = 60,
        SeventyFivePerSecond = 75, 
        NineteenPerSecont = 90,
        OneHundreedPerSecond = 100
    }

    public enum ThinkStates_INV
    {
        Idle,
        Walk,
        Chase,
        Attack,
        Flinch,
        FlinchForward,
        Death
    }

    public enum ThinkStates
    {
        Idle,
        Walk,
        Chase,
        Attack,
        AttackQTE,
        Knocked,
        Fall,
        OnGround,
        Death
    }

    public enum OnTriggerEvent
    {
        OnEnter,
        OnExit,
        Both
    }

    public enum MainMenuStates
    {
        LangaugeChoose,
        Intro,
        Menu
    }

    public enum GameplayStates
    {
        MainMenu,
        Game,
        Pause,
        Inventory,
        Map,
        Files,
        Settings,
        Cutscene,
        MemoryBW,
        Puzzle
    }

    public enum EnemySFXTypes
    {
        Idle,
        Spot,
        Attack,
        Death,
        Impact,
        Writhe
    }

    public enum QTETypes
    {
        rapid,
        once
    }

    public enum Buttons
    {
        Cross,
        Circle,
        Square,
        Triangle
    }

    public enum Enemies
    {
        Nurse,
        StraightJacket,
        InvisibleMan,
        Carrion,
        CarrionMother,
        Ariel,
        Caliban,
        CalibanBoss,
        Momma,
        Butcher,
        SadDad,
        SadDadTentacle
    }
}
