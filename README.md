# Common Pass 

## What is Common Pass ?
- Common Pass is web app that allows user to store their credential in a systematic and secure way. More secured than you think.

## Why Common Pass ?
This is my personal experience, having account in multiple website and services, I used to use similar sets of credential for overall every services. But the problem started when there are some application which needed it's user to update their credentials for every time interval and even wrose is that it didn't allow using credentials used last five time. The confusion started from there.
  
 And don't get me started on fingerprint logins. Sure, it's convenient, but it's so easy that I've practically forgotten what a password even looks like.
  
- So, Common Pass is a Web application which can be accessed through any browser (Pc or Mobile) where you can keep your credential safe and access any time you forget you credentials.
- The credentials will be fully encrypted. Even the Encryption key to encrypt the users Credential is entered by user themself.

## What can you do in Common Pass ?
- User can register easily and login into the system.
- User can set their own encryption key for encrypting their credential.
- Add and Update Account Details with User Name and Credentials.
- User can also export all their account details to an excel sheet by entering their pin.

## What if you forget your password of Common Pass ?
- Ok you are using Common Pass for keeping track of your credentials and now you forget the credential of Common Pass. Don't worry, Common Pass has an easy Forget Password feature by which you will easily get your new password for Common Pass to login into and update your password after login.

# Behind the scenes of Common Pass.
## Architecture, pattern , technologies 
- Clean Architecture is used.
- Unit of work and Repository pattern is used.
- dot net core 7 with MySQL

## System Installation
- Clone the Repository
- Setup Database Connection In appsetting.json file as per your system credentials
   (Server=localhost;Database=commonpass;Uid=root;Pwd=;)
-  Add a databse name as appsetting (here commonpass)
- Run Command 'update-database' in terminal targeting InfrastructureModule to create database table structure.
- You can Build and run the application now.
- Default Super-Admin Credentials (email : admin@gmail.com, pass: admin)
- After admin login, SU will be able create and assign role and permission.

# Common Pass Guide

## Login And Register Section
- General User can create their account in Common Pass by clicking the register.
![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/5ae880e9-8ba2-4ca4-82fd-96bbb669d9ab)
- Fill the form and Register new account with general user access.
  ![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/c64953df-545b-4d9e-b322-a217b569f088)
- After successfull login user will be directed to dashboard.

## Setting Section
- On top right you will find profile section.
  
![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/92e7394f-5cfc-4a6b-b906-6abe1429fdbc)

- You can perform these action.
- Main thing here is setting section.

![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/20b186d0-dda2-47e9-aeea-b48a6b02a47f)

- Here Encryption Key is a 16 digit hexa-decimal which is used to encrypt user's credentials.
- Pin is needed while exporting Account Details.


## Main Deals
- In left side you will find Menu Bar.
  
![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/2977fc71-3fc9-416c-9f6e-f691ccfd6b7e)

- Click on the Accounts and you will be redirected to your account and credentials section.

![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/b4c85c4b-d229-4d8b-af22-1e8c5498ffa0)

- You can Add/Update Accounts By clicking Add Button and fill the data and save it.
  
![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/b38c588d-5217-440b-b400-cb52bac989b4)

- The Pasword will be hidden by default. If you are thinking you can inspect element and change type password to text well you can try but you won't get the password.
- The only way to see your password is to click the eye icon.

![image](https://github.com/SilentCoder52626/CommonPass/assets/31434009/58c490b7-3ea2-41d4-9f37-1efb59dcf31b)

  And you will be able to see the password.
  
- You can click the eye icon again to hide the password.

  ## That's all for the main guides you'll need. Other You can go through yourself.

#Contributing

You think you can add some feature or you found a bug and want to fix it. No problem.
Just Follow this step, It's easy.

1. Create a Issues.
2. Fork the repository.
3. Create a new branch: `git checkout -b feat/name (fix/name)`
4. Make changes and commit: `git commit -am 'Add new feature'`
5. Push to the branch: `git push origin feature-name`
6. Submit a pull request.

I will review ASAP and merge it to the main branch and welcome you as a contributor. :D 
But, Make sure to follow the pattern and convention used.

# Contact Me
If you encounter any issues or have questions, feel free to open an issue or contact me directly.

Email - commonkhadka@gmail.com

For more information, visit my personal website: [Kaman Khadka Portfolio](https://kamankhadka.com.np)

# Enjoy.








 








  
