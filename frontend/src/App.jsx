import React, {useState, useCallback} from 'react';
import {
    BrowserRouter as Router, 
    Route, 
    Routes, 
    Link
} from 'react-router-dom';
import {
    AppBar, Toolbar, Typography, Button, IconButton, useMediaQuery, useTheme,
    Drawer, List, ListItemButton, ListItemText, Container, Box
} from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import HomePage from './pages/HomePage';
import CashierPage from './pages/CashierPage.jsx';
import AnimalKeeperPage from './pages/AnimalKeeperPage.jsx';
import VisitorPage from './pages/VisitorPage.jsx';

function App() {
    const theme = useTheme();
    const isSmallScreen = useMediaQuery(theme.breakpoints.down('sm'));
    const [drawerOpen, setDrawerOpen] = useState(false);

    const handleDrawerToggle = useCallback(() => {
        setDrawerOpen(prev => !prev);
    }, []);

    const closeDrawer = useCallback(() => {
        setDrawerOpen(false);
    }, []);

    const menuItems = [
        {text: 'Home', link: '/'},
        {text: 'Visitor', link: '/Visitor'},
        {text: 'Animal keeper', link: '/AnimalKeeper'},
        {text: 'Cashier', link: '/Cashier'},
    ];

    return (
        <Router>
            <AppBar position="static" sx={{backgroundColor: theme.palette.primary.dark}}>
                <Toolbar>
                    {isSmallScreen && (
                        <IconButton edge="start" color="inherit" aria-label="menu" onClick={handleDrawerToggle}>
                            <MenuIcon/>
                        </IconButton>
                    )}
                    <Typography variant="h6" sx={{flexGrow: 1}}>
                        Zoo App
                    </Typography>
                    {!isSmallScreen && (
                        <Box sx={{display: 'flex'}}>
                            {menuItems.map(item => (
                                <Button key={item.text} color="inherit" component={Link} to={item.link}>
                                    {item.text}
                                </Button>
                            ))}
                        </Box>
                    )}
                </Toolbar>
            </AppBar>
            {isSmallScreen && (
                <Drawer anchor="left" open={drawerOpen} onClose={closeDrawer}>
                    <List>
                        {menuItems.map(item => (
                            <ListItemButton button key={item.text} component={Link} to={item.link}
                                            onClick={closeDrawer}>
                                <ListItemText primary={item.text}/>
                            </ListItemButton>
                        ))}
                    </List>
                </Drawer>
            )}
            <Container>
                <Routes>
                    <Route path="/" element={<HomePage/>} exact/>
                    <Route path="/Visitor" element={<VisitorPage/>}/>
                    <Route path="/Animal keeper" element={<AnimalKeeperPage/>}/>
                    <Route path="/Cashier" element={<CashierPage/>}/>
                </Routes>
            </Container>
        </Router>
    );
}

export default App;