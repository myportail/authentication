#
# run
#    doctl kubernetes cluster kubeconfig save kubtest
#
#   to init kubectl config
#


# Set the variable value in *.tfvars file
# or using environment variable TF_VAR_do_token
# or using -var="do_token=..." CLI option
variable "do_token" {}

# Configure the DigitalOcean Provider
provider "digitalocean" {
  token = var.do_token
}

# Create a web server
# resource "digitalocean_droplet" "web" {
#   image  = "ubuntu-18-04-x64"
#   name   = "web-1"
#   size   = "s-1vcpu-1gb"
#   region = "tor1"
# }

resource "digitalocean_kubernetes_cluster" "kubtest" {
  name    = "kubtest"
  region  = "tor1"
  # Grab the latest version slug from `doctl kubernetes options versions`
  version = "1.16.6-do.0"

  node_pool {
    name       = "worker-pool"
    size       = "s-1vcpu-2gb"
    node_count = 2
  }
}

data "digitalocean_droplet" "kubtest" {
  name = digitalocean_kubernetes_cluster.kubtest.node_pool[0].nodes[0].name
}

output "kub_server_name" {
   value = digitalocean_kubernetes_cluster.kubtest.name
}

output "kub_droplet_ip" {
  value = data.digitalocean_droplet.kubtest.ipv4_address
}

resource "digitalocean_record" "www" {
  domain = "danny-thibaudeau.ca"
  type = "A"
  name = "dev.authdb"
  value = data.digitalocean_droplet.kubtest.ipv4_address
  ttl = "60"
}
