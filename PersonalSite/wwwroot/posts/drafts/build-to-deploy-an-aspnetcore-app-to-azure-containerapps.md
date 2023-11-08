---
title: How to deploy an ASP.NET Core app to Azure Container Apps using GitHub Actions
published: 2023-09-12
tags: [.NET, ASP.NET Core, Cloud, Web development, Azure, GitHub, GitHub Actions, CI]
---

Containerizing an ASP.NET Core app is easy. To build and deploy it to Azure using GitHub Actions is also.

Continuous Integration (CI)

## What are GitHub Actions?

GitHub Actions is a build automation system that is built into GitHub.

You define a workflow in a ``yaml`` file in your repository. GitHub picks that file up and can run it either when code has been committed, or when a PR has been accepted.

Under the hood it spins up a container, based on a specified image, in which you run the workflow that you have defined.

A workflow is typically made out of pre-defined building blocks, called "Actions". 

Besides actions for checking out the source code - there are actions that download particular SDKs, builds your app using an SDK, and actions that deploy the app to a specific cloud service.

With a GitHub Free plan, you get 500 MBs of storage and 2,000 minutes of build time for free. _(September 2023)_

So GitHub Actions is virtually free.

## Why Azure Container Apps?

Azure Container Apps is a managed Kubernetes services that integrates with eliminates the need to manually configure a Kubernetes cluster. You essentially just deploy your Docker image.

It has built-in support for load balancing and handles replicas. It is very easy to configure.

You can scale an app down to 0 - meaning that it will not use any resources, and then subsequently not cost any money - which is particularly good when you are developing and testing your app.

Not just that... Azure Container Apps are billed per request, and you get 2 Million requests for free every month. Other costs may incur for the use of other services. _(September 2023)_

So using Azure Container apps is a great choice when starting your projects, or when you are just exploring and learning about what Azure has to offer.

## Create an ASP.NET Core project

### Add Dockerfile

## Set up Azure

In Azure parlance, every instance of a service is referred to as a "Resource".

### Install Azure CLI

###

### Create Resource group

A resource group is a logical grouping of resources in Azure (services).

Make sure you pick your subscription.

### Create Container Registry

The Container Registry will store all of your Docker images.

Enable Managed Identity so that we can set permissions for password-less identification between resources.

### Create an Container App

Not to be confused with Container.

Enable Managed Identity

In order for the Container App to be able to pull the image from the Container Registry, we will have to assign the right permissions at the Registry. 

### Generate Azure credentials

Used by GitHub Actions. Requires Azure CLI.

```
az login
```

Run this: 

(Substitute with your Subscription Id and Resource Group)

```
az ad sp create-for-rbac \
  --name my-app-credentials \
  --role contributor \
  --scopes /subscriptions/<subscription-id>/resourceGroups/<resource-group>\
  --sdk-auth \
  --output json
```

This will output JSON with parameters to be copied to GitHub secrets.


## Deploy with GitHub Actions

### Set up secrets in GitHub

Settings > Security > Secrets and variables > Actions

Repository secrets belong to the repository.



These are the secrets

* ``AZURE_CREDENTIALS`` - The JSON output in step "Generate Azure Credentials"
* ``REGISTRY_USERNAME`` - Azure Client Id
* ``REGISTRY_PASSWORD`` - Azure Client Secret
* ``RESOURCE_GROUP`` - the name of your Azure Resource Group
* ``REGISTRY_LOGIN_SERVER`` - the host name of your Azure Container Registry

Once you have entered the secrets you can never see them again. You can only change their values. 

Some of them can be retrieve again from their sources, but otherwise, if you keep them, you should make sure to protect them.


### Add workflow

In the folder ``.github/workflows`` you create a YAML file ``main.yaml``. The name of the file doesn't really matter.

Add this to the file:

```yaml
on:
  push:
    branches:
      - main
    paths-ignore:
      - '**/README.md'
      - '**/LICENSE.md'
      
name: Build image and Deploy container

jobs:
    build-and-deploy:
        runs-on: ubuntu-latest
        steps:
        # checkout the repo
        - name: 'Checkout GitHub Action'
          uses: actions/checkout@main
          
        - name: 'Login via Azure CLI'
          uses: azure/login@v1
          with:
            creds: ${{ secrets.AZURE_CREDENTIALS }}
        
        - name: 'Build and push image'
          uses: azure/docker-login@v1
          with:
            login-server: ${{ secrets.REGISTRY_LOGIN_SERVER }}
            username: ${{ secrets.REGISTRY_USERNAME }}
            password: ${{ secrets.REGISTRY_PASSWORD }}
        - run: |
            docker build . -f src/Server/Dockerfile -t ${{ secrets.REGISTRY_LOGIN_SERVER }}/blazor8app:${{ github.sha }}
            docker push ${{ secrets.REGISTRY_LOGIN_SERVER }}/blazor8app:${{ github.sha }}
            
        - name: 'Deploy Container App'
          uses: azure/container-apps-deploy-action@v1
          with:
             registryUrl: ${{ secrets.REGISTRY_LOGIN_SERVER }}
             registryUsername: ${{ secrets.REGISTRY_USERNAME }}
             registryPassword: ${{ secrets.REGISTRY_PASSWORD }}
             imageToDeploy: ${{ secrets.REGISTRY_LOGIN_SERVER }}/blazor8app:${{ github.sha }}
             containerAppName: blazor8app
             resourceGroup: ${{ secrets.RESOURCE_GROUP }}
             location: 'Sweden Central'
             targetPort: 8080
```

We are defining the job ```run-and-deploy```, which will run on ``ubuntu-latest``.

We then have the following steps:

1. Checkout our code, using the ``actions/checkout@main`` action.
2. Login to Azure using the Azure CLI.
3. Build the Docker image, and push it to our Container Registry.
4. Pull and deploy the Docker image to the specified Azure Container Apps instance.

The ``azure/container-apps-deploy-action@v1`` is able to build the Docker image, but we do it in a separate step (3).


Provided that all of your variables are correct, when you check this file in, GitHub Actions will successfully run. 

And your will have been deployed to Container Apps.

## Conclusion

You have now.
Every commiy