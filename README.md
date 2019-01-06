# getter-setter-expression

Simple utility that allows calling getters and setters of properties (or nested properties) using Linq Expression.

Given the following class:
```csharp
public class Human
{
    public string Name { get; set; }
    
    public int Age { get; set; }
    
    public Human GrandParent { get; set; }
}
```

We can call getter:
```csharp
var age = GetSetUtility.Get((Human x) => x.GrandParent.Age)(person);
```

We can call setter
```csharp
GetSetUtility.Set((Human x) => x.GrandParent.Age)(person, updatedAge);   // void
```

