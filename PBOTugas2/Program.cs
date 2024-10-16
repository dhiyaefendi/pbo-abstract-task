using System;

class Program
{
    static void Main(string[] args)
    {
        Robot robot1 = new RobotBiasa("Robot A", 100, 10, 20);
        Robot robot2 = new RobotBiasa("Robot B", 100, 5, 15);
        BosRobot bosRobot = new BosRobot("Bos Robot", 200, 15, 25, 5);

        IKemampuan perbaikan = new Perbaikan(30, 2);
        IKemampuan seranganListrik = new SeranganListrik(25, 3);
        IKemampuan seranganPlasma = new SeranganPlasma(40, 4);
        IKemampuan pertahananSuper = new PertahananSuper(10, 5);

        robot1.GunakanKemampuan(perbaikan);
        robot1.CetakInformasi();

        robot1.Serang(robot2);
        robot2.CetakInformasi();

        bosRobot.Diserang(robot1);
        bosRobot.CetakInformasi();

        for (int i = 0; i < 5; i++)
        {
            perbaikan.UpdateCooldown();
            seranganListrik.UpdateCooldown();
            seranganPlasma.UpdateCooldown();
            pertahananSuper.UpdateCooldown();
        }

        robot2.GunakanKemampuan(seranganListrik);
        robot2.CetakInformasi();

        robot1.GunakanKemampuan(pertahananSuper);
        robot1.CetakInformasi();
    }
}

public interface IKemampuan
{
    void Gunakan(Robot target);
    bool BisaDigunakan();
    void UpdateCooldown();
}

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    protected Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public void Serang(Robot target)
    {
        int damage = Serangan - target.Armor;
        if (damage > 0)
        {
            target.Energi -= damage;
            Console.WriteLine($"{Nama} menyerang {target.Nama} dan menyebabkan {damage} damage.");
        }
        else
        {
            Console.WriteLine($"{Nama} menyerang {target.Nama} tetapi tidak menyebabkan damage.");
        }
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }
}

public class RobotBiasa : Robot
{
    public RobotBiasa(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan)
    {
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        if (kemampuan.BisaDigunakan())
        {
            kemampuan.Gunakan(this);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menggunakan kemampuan ini.");
        }
    }
}

public class BosRobot : Robot
{
    public int Pertahanan { get; set; }

    public BosRobot(string nama, int energi, int armor, int serangan, int pertahanan)
        : base(nama, energi, armor, serangan)
    {
        Pertahanan = pertahanan;
    }

    public void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan - (Armor + Pertahanan);
        if (damage > 0)
        {
            Energi -= damage;
            Console.WriteLine($"{Nama} diserang oleh {penyerang.Nama} dan menerima {damage} damage.");
        }
        else
        {
            Console.WriteLine($"{Nama} diserang tetapi tidak menerima damage.");
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah mati.");
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        if (kemampuan.BisaDigunakan())
        {
            kemampuan.Gunakan(this);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menggunakan kemampuan ini.");
        }
    }
}

public class Perbaikan : IKemampuan
{
    public int HealAmount { get; private set; }
    public int Cooldown { get; private set; }
    private int lastUsedTime;

    public Perbaikan(int healAmount, int cooldown)
    {
        HealAmount = healAmount;
        Cooldown = cooldown;
        lastUsedTime = 0;
    }

    public void Gunakan(Robot target)
    {
        target.Energi += HealAmount;
        Console.WriteLine($"{target.Nama} telah diperbaiki dan sekarang memiliki {target.Energi} energi.");
        lastUsedTime = 0;
    }

    public bool BisaDigunakan()
    {
        return lastUsedTime >= Cooldown;
    }

    public void UpdateCooldown()
    {
        lastUsedTime++;
    }
}

public class SeranganListrik : IKemampuan
{
    public int Damage { get; private set; }
    public int Cooldown { get; private set; }
    private int lastUsedTime;

    public SeranganListrik(int damage, int cooldown)
    {
        Damage = damage;
        Cooldown = cooldown;
        lastUsedTime = 0;
    }

    public void Gunakan(Robot target)
    {
        target.Energi -= Damage;
        Console.WriteLine($"{target.Nama} terkena serangan listrik dan kehilangan {Damage} energi.");
        lastUsedTime = 0; 
    }

    public bool BisaDigunakan()
    {
        return lastUsedTime >= Cooldown;
    }

    public void UpdateCooldown()
    {
        lastUsedTime++;
    }
}

public class SeranganPlasma : IKemampuan
{
    public int Damage { get; private set; }
    public int Cooldown { get; private set; }
    private int lastUsedTime;

    public SeranganPlasma(int damage, int cooldown)
    {
        Damage = damage;
        Cooldown = cooldown;
        lastUsedTime = 0;
    }

    public void Gunakan(Robot target)
    {
        target.Energi -= Damage;
        Console.WriteLine($"{target.Nama} terkena serangan plasma dan kehilangan {Damage} energi.");
        lastUsedTime = 0; 
    }

    public bool BisaDigunakan()
    {
        return lastUsedTime >= Cooldown;
    }

    public void UpdateCooldown()
    {
        lastUsedTime++;
    }
}

public class PertahananSuper : IKemampuan
{
    public int ArmorBoost { get; private set; }
    public int Cooldown { get; private set; }
    private int lastUsedTime;

    public PertahananSuper(int armorBoost, int cooldown)
    {
        ArmorBoost = armorBoost;
        Cooldown = cooldown;
        lastUsedTime = 0;
    }

    public void Gunakan(Robot target)
    {
        target.Armor += ArmorBoost;
        Console.WriteLine($"{target.Nama} meningkatkan armor dengan {ArmorBoost}.");
        lastUsedTime = 0;
    }

    public bool BisaDigunakan()
    {
        return lastUsedTime >= Cooldown;
    }

    public void UpdateCooldown()
    {
        lastUsedTime++;
    }
}
