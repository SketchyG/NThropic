# CodeAnalyzer

CodeAnalyzer is a command-line application that analyzes code files and generates a summary in JSON format. It uses the NThropic library to interact with the Claude API and its agents.

## Building the Application

To build the application, navigate to the `Demos/CodeAnalyzer` directory and run the following command:

```sh
dotnet build
```

## Running the Application

To run the application, use the following command:

```sh
dotnet run --input <input-file-path> --output <output-file-path>
```

Replace `<input-file-path>` with the path to the input code file and `<output-file-path>` with the path to the output JSON file.

## Input and Output File Formats

### Input File Format

The input file should be a plain text file containing the code to be analyzed.

### Output File Format

The output file will be a JSON file with the following structure:

```json
{
  "Summary": "This is a summary of the input file content.",
  "Details": "The detailed content of the input file."
}
```

## Dependencies

The application relies on the following projects and packages:

- `ClaudeApi`
- `ClaudeApi.Agents`
- `System.CommandLine`
- `Newtonsoft.Json`

Make sure to restore the necessary packages before building the application:

```sh
dotnet restore
```
