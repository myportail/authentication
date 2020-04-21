export class ActionsBase<IDS> {
    
    public constructor(ids: string[]) {
        this._prefix = this.constructor.name.toUpperCase();
        ids.forEach(v => this.defineId(v));
    }
    
    public get ids(): IDS {
        return this._ids as any;
    }
    
    protected defineId(name: string) {
        this._ids[name] = `${this._prefix}_${name.toUpperCase()}`;
    }
    
    private readonly _prefix: string = '';
    
    private _ids: {[key: string]: string} = {};
}
