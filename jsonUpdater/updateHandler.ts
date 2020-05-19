import * as fs from 'fs';

export class UpdateHandler {
    public handle(args: any) {
        const filename = args.file;
        
        if (!!filename) {
            this.updateFile(filename);
        }
        else {
            console.error('file parameter is required for update');            
        }
    }
    
    private updateFile(filename: string) {
        try {
            console.log(`updating file : ${filename}`);

            let content = fs.readFileSync(filename, 'utf8')
                .replace(/[\r\n]/g, '')
                .trim();
            let jsonContent = JSON.parse(content);

            console.log(content);
        }
        catch (ex) {
            console.error(ex);
        }
    }
}
