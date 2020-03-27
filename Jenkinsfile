def image

pipeline {
    agent any

    stages {
        stage('checkout') {
            steps {
                git url: 'https://github.com/myportail/authentication'
            }
        }
        
        stage('build authentication-init') {
            steps {
                script {
                    image = docker.build("myportail/authentication-init:1.0.${env.BUILD_ID}", "-f ./Docker/authInit/Dockerfile .")
                }
            }
        }
        
        stage('push authentication-init') {
            steps {
                script {
                    docker.withRegistry("", "dockerhub") {
                        image.push()
                    }
                }
            }
        }
    }
}
