Emkay.S3
========

This package contains MSBuild tasks for Amazon S3, which provides the possibility to upload files, enumerate buckets, enumerate the content of a specific 'subfolder', delete buckets and delete files from specific subfolders.

##Download

The Emkay.S3 library is available on nuget.org via package name Emkay.S3.

To install it, run the following command in the Package Manager Console

	PM> Install-Package Emkay.S3

More information about NuGet package avaliable at [https://nuget.org/packages/Emkay.S3](https://nuget.org/packages/Emkay.S3)

##Getting Started

TODO: Bit on installing via nuget at build time 

In order to use the tasks in your project, you need to import the Emkay.S3.Tasks.targets file. Maybe you need to adjust the paths to your needs.

    <Import Project="Emkay.S3.Tasks.targets"/>

Emkay S3 file publisher is an **MSBuild task** which can be used for publishing a file or set of files to S3. By default the files will be publically accessible. The following target will upload `file.txt` to `path/within_S3/file.txt`:

    <Target Name="S3_upload">
      <Message Text="Publishing to S3 ..." />
      
      <Message Text="Source folder: $(source)"/>
      <Message Text="Bucket: $(S3_bucket)"/>
      <Message Text="Destination folder: $(S3_subfolder)"/>
      
      <PublishFiles
        Key="$(aws_key)"
        Secret="$(aws_secret)"
        SourceFiles="path\to\file.txt"
        Bucket="$(aws_s3_bucket)"
        DestinationFolder="path/within_S3" />
  	</Target>

### Uploading a folder

You can also recursively publish an entire folder:

      <ItemGroup>
        <UploadFiles Include="localpath\**\*.*" />
      </ItemGroup>
      <PublishFiles
        Key="$(aws_key)"
        Secret="$(aws_secret)"
        SourceFiles="@(UploadFiles)"
        Bucket="$(aws_s3_bucket)"
        DestinationFolder="path/within_S3" />


### Headers

You can set custom headers by using ItemGroup metadata:

      
      <ItemGroup>
        <UploadFiles Include="localpath\**\css\**\*.css">
          <Content-Type>text/css</Content-Type>
          <Content-Encoding>gzip</Content-Encoding>
        </UploadFiles>
        <UploadFiles Include="localpath\**\**\*.json" Exclude="@(UploadFiles)">
          <Content-Type>application/json</Content-Type>
        </UploadFiles>
        <UploadFiles Include="localpath\**\*.*" Exclude="@(UploadFiles)" />
      </ItemGroup>
      
## License
The source code is available under the [MIT license](http://opensource.org/licenses/mit-license.php).

