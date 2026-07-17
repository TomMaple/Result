# Result&lt;T&gt;.Implicit(T to Result&lt;T&gt;) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L361" target="_blank">Result.cs</a>

Defines an implicit conversion of a given value of the type `T` to a successful [Result&lt;T&gt;](ResultT.md) (with a value), with the provided value.

```csharp
public static implicit operator Result<T>(T value);
```

### Type Parameters
#### `T`
The type of the value associated with a successful [Result&lt;T&gt;](ResultT.md).

### Parameters
#### `value` T
The value to implicitly convert.

### Returns
[Result&lt;T&gt;](ResultT.md)

A new instance of the [Result&lt;T&gt;](ResultT.md) record (with a value) that represents a successful operation with the provided *value* of the type `T`.

## Remarks
> [!NOTE]
> This is a recommended way to create a new successful [Result&lt;T&gt;](ResultT.md) instance (with a value). Another way is to use the [FromValue(T)](ResultT_FromValue.md) static method.

A `null` *value* is permitted when `T` is nullable (a nullable reference type or `Nullable<T>`); the resulting [Result&lt;T&gt;](ResultT.md) is successful and its [Value](ResultT_Value.md) is `null`.

## Examples
```csharp
Result<User> result = new User("John", "Doe");
```
