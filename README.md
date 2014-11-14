Emkay.S3
========

This package contains a small wrapper for Amazon S3. It provides possibility to upload files, upload the content of a folder (including subfolders!), enumerate buckets, enumerate the content of a specific 'subfolder', delete buckets and delete files from specific subfolders.
All those are also available via MSBuild tasks which are also part of this package. Which is the main motivation of this project.

##Download

The Emkay.S3 library is available on nuget.org via package name Emkay.S3.

To install it, run the following command in the Package Manager Console

	PM> Install-Package Emkay.S3

More information about NuGet package avaliable at [https://nuget.org/packages/Emkay.S3](https://nuget.org/packages/Emkay.S3)

##Getting Started

In order to use the tasks in your project, you need to import the Emkay.S3.Tasks.targets file. Maybe you need to adjust the paths to your needs.

	<PropertyGroup>
    	<EmkayS3ClassLibrary>$(MSBuildStartupDirectory)\Lib\Emkay.S3.dll</EmkayS3ClassLibrary>
  	</PropertyGroup>
	<UsingTask AssemblyFile="$(EmkayS3ClassLibrary)" TaskName="PublishFolder" />
	<UsingTask AssemblyFile="$(EmkayS3ClassLibrary)" TaskName="PublishFiles" />

Emkay S3 folder publisher is an **MSBuild task** which can be used for publishing recursively the content of a folder to your S3 bucket.
After that you can publish the content of a source folder to S3 by using this statement inside an MSBuild target. By default the files will be public available.

    <Target Name="S3_upload" DependsOnTargets="Publish">
    	<Message Text="Publishing to S3 ..." />
    
    	<Message Text="Source folder: $(source)"/>
    	<Message Text="Bucket: $(S3_bucket)"/>
    	<Message Text="Destination folder: $(S3_subfolder)"/>

    	<PublishFolder
      		Key="$(S3_key)"
      		Secret="$(S3_secret)"
      		SourceFolder="$(source)"
      		Bucket="$(S3_bucket)"
      		DestinationFolder="$(S3_subfolder)" />
  	</Target>

PublishFolderWithHeaders example:

    <Target Name="S3_upload" DependsOnTargets="Publish">
	  <ItemGroup>
		<Directories Include="css;scripts;">
			<Content-Type>text/css</Content-Type>
			<Content-Encoding>gzip</Content-Encoding>
		</Directories>
	  </ItemGroup> 

		<PublishFolderWithHeaders
			Key= "(Key)"
			Secret= "(Secret)"
			SourceFolders = $(Directories)
			Bucket="$(Bucket)"
			DestinationFolder="$(DestinationFolder)" />
  	</Target>

## License
The source code is available under the [MIT license](http://opensource.org/licenses/mit-license.php).

