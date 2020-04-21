class ServicesProvider {
    
    public static get instance(): ServicesProvider {
        if (ServicesProvider._instance == null) {
            ServicesProvider._instance = new ServicesProvider();
        }
        
        return ServicesProvider._instance;
    }
    
    public add(description: {}) {
        console.log(description);
    }
    
    private constructor() {
    }
    
    private static _instance: ServicesProvider;
    private mapping = {};
}

export default ServicesProvider;