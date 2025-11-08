abp install-libs

cd src/VuDrive.DbMigrator && dotnet run && cd -

cd src/VuDrive.Blazor && dotnet dev-certs https -v -ep openiddict.pfx -p e6e57820-fcd7-4fce-a91c-74472d39adab




exit 0