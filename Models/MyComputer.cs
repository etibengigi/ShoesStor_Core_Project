namespace ShoesStor.Models;

public class MyShoes
{
    public int Id { get; set; }

    public int UserId{get;set;}

    public string companyName { get; set; }

    public bool IsStationary {get; set;}
    
    public string color {get; set;}
}