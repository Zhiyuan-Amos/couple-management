# Style code with EditorConfig

Date: 2022-05-08

## Status

Accepted

## Context

There are 2 popular ways to style a .NET codebase.

1. Specifying the code styles in the form of `.editorconfig` supported configurations. These configurations take precedence over IDEs settings, which affects: 

    a. What the IDE flags out as `suggestion`, `warning` and `error`.
    a. How the IDE's auto-formatter formats the code.

    This file is compatible with multiple text editors & IDEs such as Visual Studio, Jetbrains Rider & Visual Studio Code, so contributors can use different IDEs and still have the code styles enforced.

    Key caveat is that IDEs do not necessarily honour all configurations. It seems that Visual Studio honours more configurations than Rider. For example, Visual Studio would correctly reflect an error, but not Rider, for the configuration `dotnet_naming_rule.public_fields_should_be_pascalcase.severity = error`.

    Note: As there are many configurations (perhaps > 500), specifying all of them leads to a really bloated `.editorconfig` ([example](https://github.com/meziantou/CsharpProjectTemplate/blob/main/.editorconfig)), making it hard to understand. Therefore, settings with default values matching the codebase should not be included. Also, as severity settings are not frequently referred to, they should not be included as well. (Most of the settings specified are settings that do not have default values, which I personally found it odd since these settings are pretty standard and should have default values as well. Ideally, .NET creates a model `.editorconfig` file which we could reference from, but as of today, no such file exists.)

2. Configuring the IDE's auto-formatter.

## Decision

Use EditorConfig because: 

1. It is compatible with multiple text editors & IDEs.
2. Non-IDE specific code formatting tools such as [dotnet format](https://github.com/dotnet/format) or [CleanupCode](https://www.jetbrains.com/help/rider/CleanupCode.html) are also only compatible with EditorConfig, and they are likely to be used in the future as well to automate the code formatting process and to standardise the code formatting.

## Consequences

1. Effort required to create & maintain the `.editorconfig` when upgrading to newer .NET version (which comes with newer `dotnet format` version), which could come with additional `.editorconfig` configurations.
2. Developers can use different text editors & IDEs and still largely maintain the same style when running the editor's code formatting tool.
3. Able to integrate with command-line tools to programatically format code in the future.
