namespace ContactRegister.Domain.Entities;

public class Ddd
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	public int Code { get; set; }
	public string State { get; set; }
	public string Region { get; set; }
	public string PartitionKey { get; set; }

	public Ddd()
	{
		PartitionKey = Code.ToString();
	}
    
    public Ddd(int code, string state, string region)
    {
        Code = code;
        State = state;
        Region = region;
		PartitionKey = Code.ToString();
	}

	public bool Validate(out IList<string> errors)
	{
		bool result = true;
		errors = [];

		if (!ValidateCode(Code))
		{
			errors.Add($"{nameof(Code)} must be greater than 10 and lesser than 100");
			result = false;
		}

		if (!ValidateState(State))
		{
			errors.Add($"{nameof(State)} is invalid");
			result = false;
		}

		if (!ValidateRegion(Region))
		{
			errors.Add($"{nameof(Region)} can't be null");
			result = false;
		}

		return result;
	}

	private static bool ValidateCode(int code)
	{
		return code > 10 && code < 100;
	}

	private static bool ValidateState(string state)
	{
		return GetAllUfs().Contains(state);
	}

	private static bool ValidateRegion(string region)
	{
		return !string.IsNullOrWhiteSpace(region);
	}

	private static List<string> GetAllUfs()
	{
		var ret = new List<string>
		{
			"AC",
			"AL",
			"AM",
			"AP",			
			"BA",
			"CE",
			"DF",
			"ES",
			"GO",
			"MA",
			"MG",
			"MS",
			"MT",			
			"PA",
			"PB",
			"PE",
			"PI",
			"PR",			
			"RJ",
			"RN",
			"RO",
			"RR",
			"RS",			
			"SC",
			"SE",
			"SP",			
			"TO"
		};

		return ret;
	}
}