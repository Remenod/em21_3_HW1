using System;
using System.Linq;

namespace HW1;

enum SeatType : byte
{
    StandardSeat,
    VIPSeat,
    PremiumSeat
}
struct Seat
{
    public readonly static uint[] SeatCosts = [25,50,100];
    public SeatType Type { get; set; }
    public uint Cost { get; set; }
    public bool IsOccupied { get; set; }
    public sbyte Number { get; set; }
    public Seat(SeatType type, sbyte number)
    {
        Number = number;
        Type = type;
        Cost = Type switch 
        { 
            SeatType.StandardSeat => SeatCosts[0], 
            SeatType.VIPSeat      => SeatCosts[1], 
            SeatType.PremiumSeat  => SeatCosts[2],
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid SeatType")
        };
        var rand = new Random();
        IsOccupied = rand.Next(2) == 1;
    }

    public bool OccupieSeat(ref uint? Balance)
    {
        bool isPossible = Balance >= Cost && IsOccupied == false;
        if (isPossible)
        {
            IsOccupied = true;
            Balance -= Cost;
        }
        return isPossible;
    }

    public override string ToString()
    {
        return Type switch
        {
            SeatType.StandardSeat => (IsOccupied ? "(\U0001f7e5" : "(\U0001f7e9") + (Number < 9 ? $"0{Number + 1})" : $"{Number + 1})"),
            SeatType.VIPSeat      => (IsOccupied ? "[\U0001f7e5" : "[\U0001f7e9") + (Number < 9 ? $"0{Number + 1}]" : $"{Number + 1}]"),
            SeatType.PremiumSeat  => (IsOccupied ? "{\U0001f7e5" : "{\U0001f7e9") + (Number < 9 ? $"0{Number + 1}}}" : $"{Number + 1}}}"),
            _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, "Invalid SeatType")
        };
    }

    public static Seat[] GenerateSeatArray()
    {
        Seat[] seats = new Seat[15];
        for (sbyte i = 0; i < 15; i++)
            switch (i)
            {
                case < 5:  seats[i] = new Seat(SeatType.StandardSeat, i); break;
                case < 10: seats[i] = new Seat(SeatType.VIPSeat,      i); break;
                default:   seats[i] = new Seat(SeatType.PremiumSeat,  i); break;
            }
        return seats;
    }
}
static class Validator
{
    private static uint? BaseValidate(int limit, string input) 
    {
        if (uint.TryParse(input, out uint result))
            if (result < limit)
                return result;
        return null;
    }

    public static uint? SeatValidate(string input) => BaseValidate(16, input);
    public static uint? BalanceValidate(string input) => BaseValidate(int.MaxValue, input);
}
internal class Program
{
    static void PrintSeats(Seat[] Seats)
    {
        Console.WriteLine("Доступні місця:");
        for (int i = 0; i < Seats.Length; i++)
        {
            Console.Write(Seats[i] + " ");
            if ((i + 1) % 5 == 0)
                Console.WriteLine();
        }
    }
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;     

        inputBalance:
        Console.WriteLine("Введіть скільки ви маєте кошт:");
        var RawBalance = Console.ReadLine() ?? "0";
        uint? Balance = Validator.BalanceValidate(RawBalance);
        if (Balance == null)
        {
            Console.WriteLine("Невалідний баланс.");
            goto inputBalance;
        }

        var Seats = Seat.GenerateSeatArray();
        Console.WriteLine($"Ціни місць: \n" +
                          $"З 1  по 5  ({(SeatType)0}) - {Seat.SeatCosts[0]}\n" +
                          $"З 6  по 10 ({(SeatType)1}) - {Seat.SeatCosts[1]}\n" +
                          $"З 11 по 15 ({(SeatType)2}) - {Seat.SeatCosts[2]}\n");
        PrintSeats(Seats);

        while (!Seats.All(seat => seat.IsOccupied) && Balance > Seat.SeatCosts.Min())
        {
            inputSeat:
            Console.WriteLine("Введіть номер місця яке ви хочете купити:");
            var RawSeat = Console.ReadLine() ?? "1";
            var SeatNumber = Validator.SeatValidate(RawSeat);
            if (SeatNumber == null)
            {
                Console.WriteLine("Невалідний номер місця");
                goto inputSeat;
            }
            if (SeatNumber == 0) 
            {
                Console.WriteLine("Завершення покупок.");
                Environment.Exit(0);
            }
                
            var responce = Seats[(int)SeatNumber-1].OccupieSeat(ref Balance);
            if (responce)
            {
                Console.WriteLine("Місце успішно заброньовано.");
                Console.WriteLine($"Ваш баланс: {Balance}");
                PrintSeats(Seats);
            }
            else
                Console.WriteLine("Місце вже зайняте або у вас недостатньо коштів.");
        }
        Console.WriteLine("Всі місця заброньовані або у вас не достатньо коштів на будь яку покупку.");
    }
}