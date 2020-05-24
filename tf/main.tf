#
# run
#    doctl kubernetes cluster kubeconfig save myportail
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

resource "digitalocean_kubernetes_cluster" "myportail_k8s_dev" {
  name    = "myportail-k8s-dev"
  region  = "tor1"
  # Grab the latest version slug from `doctl kubernetes options versions`
  version = "1.17.5-do.0"

  node_pool {
    name       = "myportail-k8s-dev-pool"
    size       = "s-1vcpu-2gb"
    node_count = 2
  }
}

data "digitalocean_droplet" "myportail" {
  name = digitalocean_kubernetes_cluster.myportail_k8s_dev.node_pool[0].nodes[0].name
}

locals {
  cluster_node_ids = [for node in digitalocean_kubernetes_cluster.myportail_k8s_dev.node_pool[0].nodes: node.droplet_id]
  // kube_config = digitalocean_kubernetes_cluster.myportail_k8s_dev.kube_config.0
}

// output "kube_config" {
//   value = local.kube_config
// }

# resource "digitalocean_volume" "storage" {
#   region                  = "tor1"
#   name                    = "dbstorage"
#   size                    = 1
#   description             = "mariadb volume"
# }

# resource "digitalocean_volume_attachment" "dbattachment" {
#   droplet_id = data.digitalocean_droplet.myportail.id
#   volume_id  = digitalocean_volume.storage.id
# }

output "kub_server_name" {
   value = digitalocean_kubernetes_cluster.myportail_k8s_dev.name
}

# output "kub_droplet_ip" {
#  value = data.digitalocean_droplet.myportail.ipv4_address
# }

// resource "digitalocean_loadbalancer" "myportail_k8s_dev_lb" {
//   name   = "loadbalancer-myportail-dev"
//   region = "tor1"

//   forwarding_rule {
//     entry_port     = 80
//     entry_protocol = "http"

//     target_port     = 80
//     target_protocol = "http"
//   }

//   healthcheck {
//     port     = 30027
//     protocol = "tcp"
//   }

//   droplet_ids = local.cluster_node_ids
// }

// resource "digitalocean_record" "www" {
//   domain = "danny-thibaudeau.ca"
//   type = "A"
//   name = "myportail"
//   value = digitalocean_loadbalancer.myportail_k8s_dev_lb.ip
//   ttl = "60"
// }
