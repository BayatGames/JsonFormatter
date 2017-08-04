# JsonFormatter

Easy, Fast and Lightweight Json Formatter. (Serializer and Deserializer)

| [Getting Started](#getting-started) | [Features](#features) | [Download](#download) |
| --------------- | -------- | -------- |

## Features

All default json serializer features plus:

- [:fire: Serialize Almost Everything](#serialize-almost-everything)
- [:rocket: Surrogate Serialization](#surrogate-serialization)

### Serialize Almost Everything

When you have class look like the following:

```csharp
public class Sample {

  public int sampleField = 120;

}
```

The JsonFormatter will serializes the class public fields to:

```json
{"sample":"120"}
```

Also JsonFormatter supports **Array** (only one dimensional arrays, because of javascript support), **List**, **Dicitonary**.

### Surrogate Serialization

Want to serialize something un-serializable, use surrogates.

In the below example we will try to add a surrogate for unity Vector3 type:

```csharp
using System;
using System.Runtime.Serialization;
using UnityEngine;

public class Vector3Surrogate : ISerializationSurrogate {

  public void GetObjectData (object obj, SerializationInfo info, StreamingContext context) {
    Vector3 vector = (Vector3)obj;
    info.AddValue("x", vector.x);
    info.AddValue("y", vector.y);
    info.AddValue("z", vector.z);
  }

  public object SetObjectData (object obj, SerializationInfo info, StreamingContext, context, ISurrogateSelector selector) {
    Vector3 vector = (Vector3)obj;
    vector.x = info.GetInt32("x");
    vector.y = info.GetInt32("y");
    vector.z = info.GetInt32("z");
    return null;
  }

}
```

Now add create a surrogate selector and add the Vector3 surrogate:

```csharp

using BayatGames.Serialization.Formatters.Json;

public class Example {

  void Start () {
    
    SurrogateSelector selector = new SurrogateSelector();
    selector.AddSurrogate(typeof(Vector3), 
      new StreamingContext(StreamingContextStates.All), 
      new Vector3Surrogate ());
     JsonFormatter formatter = new JsonFormatter(selector);
     string json = formatter.Serialize(new Vector3(12, 132, 543));
     Debug.Log(json); // -> "{"x":"12","y":"132","z":"543"}"
    
  }

}
```

Also JsonFormatters supports **ISerializable** interface.

## Getting Started

- [Download JsonFormatter](#download)
- Serialize your data:

```csharp
using BayatGames.Serialization.Formatters.Json;

...

JsonFormatter.SerializeObject ("Hello World!");
```

## Download

[:sparkles: Download latest version](https://github.com/BayatGames/JsonFormatter/releases/latest)

[:fire: Download source code](https://github.com/BayatGames/JsonFormatter/archive/master.zip)

Or clone the repository:

```bash
git clone https://github.com/BayatGames/JsonFormatter.git
```

## License

MIT @ [Bayat Games](https://github.com/BayatGames)

Made with :heart: by [Bayat Games](https://github.com/BayatGames)
