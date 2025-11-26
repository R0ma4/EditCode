using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorCodeIDE.Pack_EditCode
{
    public class ProjectSystem
    {
        string standart_code_rogect(string project_name, string project_version)
        {
            return
        @"<project h:Name = 'project_name',h:language  = 'XML',h:Code = 'UNI8', version:'1.0'>
			<Direct = ''/>
			<settnega>
				<language='ru'/>
				<Pakets='false'/>
				<Git='false'/>
			</settnega>
			<Addons>
				<User>
				</User>
				<Standart>
					<FrameWorkAPI version='0.0.1'/>
					<EditCodeIDE version='0.0.1'/>
				</Standart>
			</Addons>
		</project>";
        }
    }
}
