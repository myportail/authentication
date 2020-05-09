def authInitImage
def authServiceImage

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
                    authInitImage = docker.build("myportail/authentication-init:1.0.${env.BUILD_ID}", "-f ./Docker/authInit/Dockerfile .")
                }
            }
        }
        
        stage('build authentication-service') {
            steps {
                script {
                    authServiceImage = docker.build("myportail/auth-service:1.0.${env.BUILD_ID}", "--build-arg version=1.0.${env.BUILD_ID} -f ./Docker/authService/Dockerfile .")
                }
            }
        }

        stage('push authentication-init') {
            steps {
                script {
                    docker.withRegistry("", "dockerhub") {
                        authInitImage.push()
                    }
                }
            }
        }

        stage('push authentication-service') {
            steps {
                script {
                    docker.withRegistry("", "dockerhub") {
                        authServiceImage.push()
                    }
                }
            }
        }
    }
}
