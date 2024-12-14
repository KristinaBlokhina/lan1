using System;
using System.Numerics;

interface IMyNumber<T> where T : IMyNumber<T>
{
    T Add(T b);
    T Subtract(T b);
    T Multiply(T b);
    T Divide(T b);
}

class MyFrac : IMyNumber<MyFrac>
{
    private BigInteger nom;
    private BigInteger denom;

    public MyFrac(BigInteger nom, BigInteger denom)
    {
        if (denom == 0)
        {
            throw new DivideByZeroException("Denominator cannot be zero.");
        }
        this.nom = nom;
        this.denom = denom;
        Simplify();
    }

    public MyFrac(int nom, int denom) : this(new BigInteger(nom), new BigInteger(denom)) { }

    private void Simplify()
    {
        BigInteger gcd = BigInteger.GreatestCommonDivisor(nom, denom);
        nom /= gcd;
        denom /= gcd;
        if (denom < 0)
        {
            nom = -nom;
            denom = -denom;
        }
    }

    public MyFrac Add(MyFrac that)
    {
        return new MyFrac(this.nom * that.denom + that.nom * this.denom, this.denom * that.denom);
    }

    public MyFrac Subtract(MyFrac that)
    {
        return new MyFrac(this.nom * that.denom - that.nom * this.denom, this.denom * that.denom);
    }

    public MyFrac Multiply(MyFrac that)
    {
        return new MyFrac(this.nom * that.nom, this.denom * that.denom);
    }

    public MyFrac Divide(MyFrac that)
    {
        if (that.nom == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return new MyFrac(this.nom * that.denom, this.denom * that.nom);
    }

    public override string ToString()
    {
        return $"{nom}/{denom}";
    }
}

class MyComplex : IMyNumber<MyComplex>
{
    private double re;
    private double im;

    public MyComplex(double re, double im)
    {
        this.re = re;
        this.im = im;
    }

    public MyComplex Add(MyComplex that)
    {
        return new MyComplex(this.re + that.re, this.im + that.im);
    }

    public MyComplex Subtract(MyComplex that)
    {
        return new MyComplex(this.re - that.re, this.im - that.im);
    }

    public MyComplex Multiply(MyComplex that)
    {
        return new MyComplex(this.re * that.re - this.im * that.im, this.re * that.im + this.im * that.re);
    }

    public MyComplex Divide(MyComplex that)
    {
        double denominator = that.re * that.re + that.im * that.im;
        if (denominator == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return new MyComplex((this.re * that.re + this.im * that.im) / denominator,
                              (this.im * that.re - this.re * that.im) / denominator);
    }

    public override string ToString()
    {
        return $"{re} + {im}i";
    }
}

class Program
{
    static void TestAPlusBSquare<T>(T a, T b) where T : IMyNumber<T>
    {
        Console.WriteLine($"=== Testing (a+b)^2 = a^2 + 2ab + b^2 with a = {a}, b = {b} ===");
        T aPlusB = a.Add(b);
        Console.WriteLine($"(a + b) = {aPlusB}");
        Console.WriteLine($"(a+b)^2 = {aPlusB.Multiply(aPlusB)}");

        T aSquared = a.Multiply(a);
        T bSquared = b.Multiply(b);
        T twoAB = a.Multiply(b).Add(a.Multiply(b));

        Console.WriteLine($"a^2 = {aSquared}");
        Console.WriteLine($"b^2 = {bSquared}");
        Console.WriteLine($"2ab = {twoAB}");
        Console.WriteLine($"a^2 + 2ab + b^2 = {aSquared.Add(twoAB).Add(bSquared)}");
    }

    static void Main(string[] args)
    {
        TestAPlusBSquare(new MyFrac(1, 3), new MyFrac(1, 6));
        TestAPlusBSquare(new MyComplex(1, 3), new MyComplex(1, 6));
        Console.ReadLine();
    }
}
