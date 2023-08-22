import { useState, useEffect } from 'react';
import { Container, Grid, Typography, Button, Box } from '@mui/material';
import TicketForm from '../components/Ticketform.jsx';
import TicketTable from '../../src/components/TicketTable';
import { fetchTickets, buyTicket, fetchTicketsByDate } from '../../api';
import Layout from "../../Layout.jsx";

function CashierPage() {
    const [tickets, setTickets] = useState([]);
    const [selectedDate, setSelectedDate] = useState(new Date().toISOString().slice(0, 16));
    const [selectedTicketType, setSelectedTicketType] = useState('');

    useEffect(() => {
        async function loadTickets() {
            const fetchedTickets = await fetchTickets();
            setTickets(fetchedTickets);
        }
        loadTickets();
    }, []);

    const handleDateChange = (event) => {
        setSelectedDate(event.target.value);
    };

    const handleTicketTypeChange = (event) => {
        setSelectedTicketType(event.target.value);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (selectedDate && selectedTicketType) {
            const newTicket = await buyTicket(selectedTicketType, selectedDate);
            setTickets(prevTickets => [...prevTickets, newTicket]);
        }


    };

    const fetchAndSetTicketsByDate = async () => {
        const fetchedTickets = await fetchTicketsByDate(selectedDate);
        setTickets(fetchedTickets);
    };



    const totalTickets = tickets.length;
    const totalPrice = tickets.reduce((sum, ticket) => sum + ticket.preis, 0);

    return (
        <Container>
            <Layout>
                <Box sx={{mt: 4, mb: 4, textAlign: 'center'}}>
                    <Typography variant="h4" gutterBottom>
                        Cashier Page
                    </Typography>
                </Box>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={6} md={4}>
                        <TicketForm
                            selectedDate={selectedDate}
                            handleDateChange={handleDateChange}
                            selectedTicketType={selectedTicketType}
                            handleTicketTypeChange={handleTicketTypeChange}
                            handleSubmit={handleSubmit}
                        />
                    </Grid>
                    <Grid item xs={12} sm={6} md={4}>
                        <Button
                            variant="contained"
                            color="secondary"
                            fullWidth
                            onClick={fetchAndSetTicketsByDate}
                            sx={{mt: 2}}
                        >
                            Retrieve tickets by date
                        </Button>
                    </Grid>
                    <Grid item xs={12}>
                        <TicketTable tickets={tickets}/>
                    </Grid>
                </Grid>
                <Box sx={{mt: 4, textAlign: 'center'}}>
                    <Typography variant="h6" gutterBottom>
                        Total number of tickets: {totalTickets}
                    </Typography>
                    <Typography variant="h6">
                        Total amount: {totalPrice.toFixed(2)} €
                    </Typography>
                </Box>
            </Layout>
        </Container>
    );
};

export default CashierPage;