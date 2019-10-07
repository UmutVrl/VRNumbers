class Randomizer
{
    public static readonly System.Random rnd;
   // Unity.Random.seed = System.DateTime.Now.Millisecond;	
    
    static Randomizer() 
    {
		rnd = new System.Random();
    }
    
    private void Count()
    {
        int randomNumber  = rnd.Next(8, 9);
        int randomNumberSecond = rnd.Next(8, 9);
       
    }
}