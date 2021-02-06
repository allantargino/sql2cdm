# Command line interface (CLI) documentation

## Options

### Input option

- **Type:** String
- **Use:** Input SQL file to be converted to CDM.
- **Value:** *i* or *input*.
- **Required:** True
- **Example:**

```console
sql2cdm -i "C:\downloads\file.sql"
// or
sql2cdm -input "C:\downloads\file.sql"
```

### Output option

- **Type:** String
- **Use:** Output CDM folder.
- **Value:** *o* or *output*.
- **Required:** False
- **Default:**: **"."** // Current folder
- **Example:**

```console
sql2cdm -o "C:\downloads\cdmFolder"
// or
sql2cdm -output "C:\downloads\cdmFolder"
```

### Timestamps option

- **Type:** Boolean
- **Use:** Defines if there are timestamps
- **Value:** *timestamps*.
- **Required:** False
- **Default:**: False
- **Example:**

```console
sql2cdm -timestamps true
// or
sql2cdm -timestamps false
```

### Virtual option

- **Type:** Boolean
- **Use:** Defines if there is a virtual partition.
- **Value:** *virtual*.
- **Required:** False
- **Default:**: False
- **Example:**

```console
sql2cdm -virtual false
// or
sql2cdm -virtual false
```

[Option("virtual", Required = false, Default = false)]
public bool HasVirtualPartition { get; set; }

### Log option

- **Type:** Integer
- **Use:** Defines the log level between 0 and 6. More information on logging [here](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-3.1).
- **Value:** *l* or *log-level*.
- **Required:** False
- **Default:**: 2
- **Example:**

```console
sql2cdm -l 2
// or
sql2cdm -log-level 4
```

## Examples

- Transform a file called file.sql to a CDM format and write the transformation at the folder "C:\downloads\cdmFolder", without a virtual partition, without timestamps and with an information log level.

```console
sql2cdm -input "C:\downloads\file.sql" -output "C:\downloads\cdmFolder"
```

- Transform a file called file.sql to a CDM format and write the transformation at the folder "C:\downloads\cdmFolder", without a virtual partition, without timestamps and with a debug log level.

```console
sql2cdm -i "C:\downloads\file.sql" -o "C:\downloads\cdmFolder" -timestamps false -virtual false -log 1
```

- Transform a file called file.sql to a CDM format and write the transformation at the folder "C:\downloads\cdmFolder", with a virtual partition, without timestamps and with a warning log level.

```console
sql2cdm -i "C:\downloads\file.sql" -o "C:\downloads\cdmFolder" -timestamps true -virtual true -log 3
```
