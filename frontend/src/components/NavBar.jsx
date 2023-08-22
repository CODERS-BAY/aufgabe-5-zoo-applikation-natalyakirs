import {useState} from 'react';
import {AppBar, Toolbar, IconButton, Typography, Drawer, List, ListItem, ListItemText} from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';

const Navbar = ({title = 'Zoo App', menuItems = [], onNavigate}) => {
    const [drawerOpen, setDrawerOpen] = useState(false);

    // Function to toggle the state of the drawer (menu)
    const handleDrawerToggle = () => {
        setDrawerOpen(!drawerOpen);
    };
    const handleNavigation = (path) => {
        onNavigate(path);
        setDrawerOpen(false);
    };

    return (
        <div>
            <AppBar position="static">
                <Toolbar>
                    <IconButton edge="start" color="inherit" aria-label="menu" onClick={handleDrawerToggle}>
                        <MenuIcon/>
                    </IconButton>
                    <Typography variant="h6">
                        {title}
                    </Typography>
                </Toolbar>
            </AppBar>
            <Drawer anchor="left" open={drawerOpen} onClose={handleDrawerToggle}>
                <List>
                    {menuItems.map(item => (
                        <ListItem button key={item.path} onClick={() => handleNavigation(item.path)}>
                            <ListItemText primary={item.label}/>
                        </ListItem>
                    ))}
                </List>
            </Drawer>
        </div>
    );
};

// proptypes validation
Navbar.propTypes = {
    title: PropTypes.string,
    menuItems: PropTypes.arrayOf(PropTypes.object),
    onNavigate: PropTypes.func,
};


export default Navbar;